using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FactoryTrafficManagement.Web.Controllers;

[ApiController]
[Route("api/sensor-events")]
public class SensorEventsController : ControllerBase
{
    private readonly ISensorEventService _sensorEventService;

    public SensorEventsController(
        ISensorEventService sensorEventService)
    {
        _sensorEventService = sensorEventService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessEvent(
        [FromBody] SensorEventRequest request)
    {
        try
        {
            await _sensorEventService.ProcessAsync(request);

            return Ok(new
            {
                message = "Sensor event processed successfully.",
                eventId = request.EventId
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