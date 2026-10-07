using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.Services;

public class JunctionService : IJunctionService
{
    private readonly IJunctionRepository _junctionRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public JunctionService(
        IJunctionRepository junctionRepository,
        IVehicleRepository vehicleRepository)
    {
        _junctionRepository = junctionRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IReadOnlyList<JunctionStatusDto>> GetAllAsync()
    {
        var junctions =
            await _junctionRepository.GetAllAsync();

        return junctions
            .Select(MapToDto)
            .ToList();
    }

    public async Task<JunctionStatusDto?> GetStatusAsync(
        string junctionId)
    {
        var junction =
            await _junctionRepository.GetByIdAsync(junctionId);

        return junction == null
            ? null
            : MapToDto(junction);
    }

    public async Task<IReadOnlyList<QueuedVehicleDto>>
        GetQueuedVehiclesAsync(string junctionId)
    {
        var vehicles =
            await _vehicleRepository
                .GetQueuedByJunctionIdAsync(junctionId);

        var now = DateTime.UtcNow;

        return vehicles
            .Select(vehicle => new QueuedVehicleDto
            {
                VehicleId = vehicle.Id,
                JunctionId = vehicle.JunctionId,
                Direction = vehicle.Direction,
                VehicleType = vehicle.Type,
                ArrivedAt = vehicle.ArrivedAt,
                WaitingSeconds = Math.Max(
                    0,
                    (now - vehicle.ArrivedAt).TotalSeconds)
            })
            .ToList();
    }

    public async Task SetModeAsync(
        string junctionId,
        JunctionModeRequest request)
    {
        var junction =
            await _junctionRepository.GetByIdAsync(
                junctionId);

        if (junction == null)
        {
            throw new InvalidOperationException(
                $"Junction '{junctionId}' was not found.");
        }

        junction.SetMode(request.Mode);

        await _junctionRepository.SaveAsync(junction);
    }

    private static JunctionStatusDto MapToDto(
        Domain.Entities.Junction junction)
    {
        return new JunctionStatusDto
        {
            JunctionId = junction.Id,
            Name = junction.Name,
            Mode = junction.Mode,
            CurrentPhase = junction.CurrentPhase,
            ControllerOnline = junction.ControllerOnline,
            PhaseStartedAt = junction.PhaseStartedAt,
            UpdatedAt = junction.UpdatedAt,

            DesiredSignals = new SignalSetDto
            {
                North = junction.DesiredNorth,
                South = junction.DesiredSouth,
                East = junction.DesiredEast,
                West = junction.DesiredWest
            },

            ActualSignals = new SignalSetDto
            {
                North = junction.ActualNorth,
                South = junction.ActualSouth,
                East = junction.ActualEast,
                West = junction.ActualWest
            }
        };
    }
}