using IzmitTransportationSystem.Models;
using IzmitTransportationSystem.Services;
using Microsoft.AspNetCore.Mvc;



namespace IzmitTransportationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportationController : ControllerBase
    {
        private readonly RoutePlannerService _routePlannerService;

        public TransportationController(RoutePlannerService routePlannerService)
        {
            _routePlannerService = routePlannerService;
        }

        [HttpPost("planjourney")]
        public ActionResult<JourneyPlanResponse> PlanJourney([FromBody] JourneyRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");

            var response = _routePlannerService.PlanJourney(request);
            return Ok(response);
        }
    }
}
