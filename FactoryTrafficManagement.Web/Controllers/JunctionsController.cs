using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FactoryTrafficManagement.Web.Controllers;

[ApiController]
[Route("api/junctions")]
public class JunctionsController : ControllerBase
{
    private readonly IJunctionService _junctionService;
    private readonly ITrafficDecisionService _trafficDecisionService;
    private readonly IAutomaticTrafficService _automaticTrafficService;

    public JunctionsController(
        IJunctionService junctionService,
        ITrafficDecisionService trafficDecisionService,
        IAutomaticTrafficService automaticTrafficService)
    {
        _junctionService = junctionService;
        _trafficDecisionService = trafficDecisionService;
        _automaticTrafficService = automaticTrafficService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var junctions =
            await _junctionService.GetAllAsync();

        return Ok(junctions);
    }

    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetStatus(
        string id)
    {
        var junction =
            await _junctionService.GetStatusAsync(id);

        if (junction == null)
        {
            return NotFound(new
            {
                message = $"Junction '{id}' was not found."
            });
        }

        return Ok(junction);
    }

    [HttpPost("{id}/transition")]
    public async Task<IActionResult> ExecuteTransition(
    string id,
    [FromServices] ITrafficTransitionService transitionService)
    {
        try
        {
            await transitionService.ExecuteAsync(id);

            return Ok(new
            {
                junctionId = id,
                message = "Safe traffic transition executed."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("{id}/queues")]
    public async Task<IActionResult> GetQueues(
        string id)
    {
        var junction =
            await _junctionService.GetStatusAsync(id);

        if (junction == null)
        {
            return NotFound(new
            {
                message = $"Junction '{id}' was not found."
            });
        }

        var vehicles =
            await _junctionService
                .GetQueuedVehiclesAsync(id);

        return Ok(vehicles);
    }

    [HttpGet("{id}/next-direction")]
    public async Task<IActionResult> GetNextDirection(
        string id)
    {
        var junction =
            await _junctionService.GetStatusAsync(id);

        if (junction == null)
        {
            return NotFound(new
            {
                message = $"Junction '{id}' was not found."
            });
        }

        try
        {
            var direction =
                await _trafficDecisionService
                    .GetNextDirectionAsync(id);

            return Ok(new
            {
                junctionId = id,
                selectedDirection = direction
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("{id}/mode")]
    public async Task<IActionResult> SetMode(
    string id,
    [FromBody] JunctionModeRequest request)
    {
        try
        {
            await _junctionService.SetModeAsync(id, request);

            return Ok(new
            {
                junctionId = id,
                mode = request.Mode,
                message = "Junction mode updated."
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
    [HttpGet("{id}/history")]
    public async Task<IActionResult> GetHistory(
    string id,
    [FromServices] IAuditRepository auditRepository)
    {
        var junction =
            await _junctionService.GetStatusAsync(id);

        if (junction == null)
        {
            return NotFound(new
            {
                message = $"Junction '{id}' was not found."
            });
        }

        var history =
            await auditRepository.GetByJunctionIdAsync(id);

        return Ok(history);
    }
    [HttpGet("{id}/dashboard")]
    public async Task<IActionResult> GetDashboard(string id)
    {
        var status =
            await _junctionService.GetStatusAsync(id);

        if (status == null)
        {
            return NotFound(new
            {
                message = $"Junction '{id}' was not found."
            });
        }

        var vehicles =
            await _junctionService
                .GetQueuedVehiclesAsync(id);

        return Ok(new
        {
            junction = status,
            vehicles = vehicles
        });
    }

    [HttpGet("{id}/automatic-transition")]
    public async Task<IActionResult> GetAutomaticTransition(
        string id)
    {
        try
        {
            var transition =
                await _automaticTrafficService
                    .EvaluateAsync(id);

            return Ok(transition);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}