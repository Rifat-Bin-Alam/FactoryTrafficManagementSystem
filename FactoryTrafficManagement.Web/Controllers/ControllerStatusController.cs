using FactoryTrafficManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FactoryTrafficManagement.Web.Controllers;

[ApiController]
[Route("api/junctions/{junctionId}/controller")]
public class ControllerStatusController : ControllerBase
{
    private readonly IControllerStatusService _statusService;

    public ControllerStatusController(
        IControllerStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpPost("offline")]
    public async Task<IActionResult> SetOffline(
        string junctionId)
    {
        try
        {
            await _statusService.SetOfflineAsync(
                junctionId);

            return Ok(new
            {
                junctionId,
                controllerOnline = false,
                mode = "Degraded"
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("online")]
    public async Task<IActionResult> SetOnline(
        string junctionId)
    {
        try
        {
            await _statusService.SetOnlineAsync(
                junctionId);

            return Ok(new
            {
                junctionId,
                controllerOnline = true,
                mode = "Automatic"
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }
}