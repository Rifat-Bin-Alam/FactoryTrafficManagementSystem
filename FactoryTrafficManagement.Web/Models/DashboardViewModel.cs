using FactoryTrafficManagement.Application.DTOs;

namespace FactoryTrafficManagement.Web.Models;

public class DashboardViewModel
{
    public JunctionStatusDto? Junction { get; set; }

    public IReadOnlyList<QueuedVehicleDto> QueuedVehicles { get; set; }
        = new List<QueuedVehicleDto>();

    public IReadOnlyList<Domain.Entities.AuditLog> History { get; set; }
        = new List<Domain.Entities.AuditLog>();
}