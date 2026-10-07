using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.Services;

public class SensorEventService : ISensorEventService
{
    private readonly ISensorEventRepository _sensorEventRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IJunctionRepository _junctionRepository;
    private readonly IAuditRepository _auditRepository;

    public SensorEventService(
        ISensorEventRepository sensorEventRepository,
        IVehicleRepository vehicleRepository,
        IJunctionRepository junctionRepository,
        IAuditRepository auditRepository)
    {
        _sensorEventRepository = sensorEventRepository;
        _vehicleRepository = vehicleRepository;
        _junctionRepository = junctionRepository;
        _auditRepository = auditRepository;
    }

    public async Task ProcessAsync(
        SensorEventRequest request)
    {
        ValidateRequest(request);

        // Idempotency:
        // The same event must never modify the queue twice.
        var existingEvent =
            await _sensorEventRepository.GetByIdAsync(
                request.EventId);

        if (existingEvent != null)
        {
            return;
        }

        var junction =
            await _junctionRepository.GetByIdAsync(
                request.JunctionId);

        if (junction == null)
        {
            throw new InvalidOperationException(
                $"Junction '{request.JunctionId}' was not found.");
        }

        var sensorEvent = new SensorEvent(
            request.EventId,
            request.JunctionId,
            request.Direction,
            request.EventType,
            request.VehicleId,
            request.VehicleType,
            request.SequenceNumber,
            request.Timestamp);

        if (request.EventType == EventType.VehicleDetected)
        {
            await ProcessVehicleDetectedAsync(
                sensorEvent,
                request);

            var audit = new AuditLog(
                request.JunctionId,
                "VEHICLE_DETECTED",
                $"Vehicle {request.VehicleId} detected from {request.Direction}.");

            await _auditRepository.AddAsync(audit);

            if (request.VehicleType == VehicleType.Emergency)
            {
                junction.SetMode(
                    JunctionMode.Emergency);

                await _junctionRepository.SaveAsync(
                    junction);

                var emergencyAudit = new AuditLog(
                    request.JunctionId,
                    "EMERGENCY_ACTIVATED",
                    $"Emergency vehicle {request.VehicleId} detected from {request.Direction}.");

                await _auditRepository.AddAsync(
                    emergencyAudit);
            }
        }
        else if (request.EventType == EventType.VehicleCleared)
        {
            await ProcessVehicleClearedAsync(
                sensorEvent,
                request);

            var audit = new AuditLog(
                request.JunctionId,
                "VEHICLE_CLEARED",
                $"Vehicle {request.VehicleId} cleared.");

            await _auditRepository.AddAsync(audit);
        }

        sensorEvent.MarkProcessed();

        await _sensorEventRepository.AddAsync(
            sensorEvent);

        await _sensorEventRepository.SaveChangesAsync();

        await _auditRepository.SaveChangesAsync();
    }

    private async Task ProcessVehicleDetectedAsync(
        SensorEvent sensorEvent,
        SensorEventRequest request)
    {
        var existingVehicle =
            await _vehicleRepository.GetByIdAsync(
                request.VehicleId);

        if (existingVehicle != null)
        {
            throw new InvalidOperationException(
                $"Vehicle '{request.VehicleId}' already exists.");
        }

        var vehicle = new Vehicle(
            request.VehicleId,
            request.JunctionId,
            request.Direction,
            request.VehicleType,
            request.Timestamp);

        vehicle.AcceptSequence(
            request.SequenceNumber);

        await _vehicleRepository.AddAsync(
            vehicle);

        await _vehicleRepository.SaveChangesAsync();
    }

    private async Task ProcessVehicleClearedAsync(
        SensorEvent sensorEvent,
        SensorEventRequest request)
    {
        var vehicle =
            await _vehicleRepository.GetByIdAsync(
                request.VehicleId);

        if (vehicle == null)
        {
            throw new InvalidOperationException(
                $"Vehicle '{request.VehicleId}' was not found.");
        }

        if (!vehicle.AcceptSequence(
                request.SequenceNumber))
        {
            throw new InvalidOperationException(
                $"Out-of-order or duplicate sequence number " +
                $"'{request.SequenceNumber}' for vehicle " +
                $"'{request.VehicleId}'.");
        }

        vehicle.MarkCleared();

        if (vehicle.Type == VehicleType.Emergency)
        {
            var junction =
                await _junctionRepository.GetByIdAsync(
                    request.JunctionId);

            if (junction != null &&
                junction.Mode == JunctionMode.Emergency)
            {
                junction.SetMode(
                    JunctionMode.Automatic);

                await _junctionRepository.SaveAsync(
                    junction);

                var audit = new AuditLog(
                    request.JunctionId,
                    "EMERGENCY_CLEARED",
                    $"Emergency vehicle {request.VehicleId} cleared. " +
                    "Returned to Automatic mode.");

                await _auditRepository.AddAsync(
                    audit);
            }
        }
    }

    private static void ValidateRequest(
        SensorEventRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.EventId))
        {
            throw new ArgumentException(
                "EventId is required.");
        }

        if (string.IsNullOrWhiteSpace(request.JunctionId))
        {
            throw new ArgumentException(
                "JunctionId is required.");
        }

        if (string.IsNullOrWhiteSpace(request.VehicleId))
        {
            throw new ArgumentException(
                "VehicleId is required.");
        }

        if (request.SequenceNumber < 0)
        {
            throw new ArgumentException(
                "SequenceNumber cannot be negative.");
        }

        if (request.Timestamp == default)
        {
            throw new ArgumentException(
                "Timestamp is required.");
        }
    }
}