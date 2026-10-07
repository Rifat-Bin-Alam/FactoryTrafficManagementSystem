using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FactoryTrafficManagement.Web.Controllers;

[ApiController]
[Route("api/junctions/{junctionId}/commands")]
public class JunctionCommandsController : ControllerBase
{
    private readonly IControllerCommandService _commandService;

    public JunctionCommandsController(
        IControllerCommandService commandService)
    {
        _commandService = commandService;
    }

    [HttpPost]
    public async Task<IActionResult> SendCommand(
        string junctionId,
        [FromBody] ControllerCommandRequest request)
    {
        request.JunctionId = junctionId;

        try
        {
            var commandId =
                await _commandService.SendCommandAsync(request);

            return Accepted(new
            {
                commandId,
                junctionId,
                message = "Controller command accepted."
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
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }
}