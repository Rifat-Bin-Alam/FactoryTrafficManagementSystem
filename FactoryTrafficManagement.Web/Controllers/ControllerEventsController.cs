using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FactoryTrafficManagement.Web.Controllers;

[ApiController]
[Route("api/controller-events")]
public class ControllerEventsController : ControllerBase
{
    private readonly IControllerEventService _controllerEventService;

    public ControllerEventsController(
        IControllerEventService controllerEventService)
    {
        _controllerEventService = controllerEventService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessControllerEvent(
        [FromBody] ControllerEventRequest request)
    {
        try
        {
            await _controllerEventService
                .ProcessAcknowledgementAsync(request);

            return Ok(new
            {
                message = request.Successful
                    ? "Controller command acknowledged."
                    : "Controller command failed.",

                commandId = request.CommandId
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
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