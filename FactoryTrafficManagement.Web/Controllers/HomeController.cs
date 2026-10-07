using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FactoryTrafficManagement.Web.Controllers;

public class HomeController : Controller
{
    private readonly IJunctionService _junctionService;
    private readonly IAuditRepository _auditRepository;

    public HomeController(
        IJunctionService junctionService,
        IAuditRepository auditRepository)
    {
        _junctionService = junctionService;
        _auditRepository = auditRepository;
    }

    public async Task<IActionResult> Index()
    {
        var junction =
            await _junctionService.GetStatusAsync("A");

        if (junction == null)
        {
            return NotFound();
        }

        var vehicles =
            await _junctionService
                .GetQueuedVehiclesAsync("A");

        var history =
            await _auditRepository
                .GetByJunctionIdAsync("A");

        var model = new DashboardViewModel
        {
            Junction = junction,
            QueuedVehicles = vehicles,
            History = history
        };

        return View(model);
    }
}