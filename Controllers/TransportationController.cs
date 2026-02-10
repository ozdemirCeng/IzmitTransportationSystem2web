using IzmitTransportationSystem.Models;
using IzmitTransportationSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;



namespace IzmitTransportationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportationController : ControllerBase
    {
        private readonly RoutePlannerService _routePlannerService;
        private readonly ILogger<TransportationController> _logger;

        public TransportationController(RoutePlannerService routePlannerService, ILogger<TransportationController> logger)
        {
            _routePlannerService = routePlannerService;
            _logger = logger;
        }

        [HttpPost("planjourney")]
        public ActionResult<JourneyPlanResponse> PlanJourney([FromBody] JourneyRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Invalid request data.");

                _logger.LogInformation("Planning journey from ({StartLat}, {StartLon}) to ({DestLat}, {DestLon})",
                    request.StartLatitude, request.StartLongitude, request.DestinationLatitude, request.DestinationLongitude);

                var response = _routePlannerService.PlanJourney(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error planning journey: {Message}", ex.Message);
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
