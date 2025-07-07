namespace IzmitTransportationSystem.Models
{
    public class JourneyPlanResponse
    {
        public string NearestStartStop { get; set; }
        public double DistanceToStartStop { get; set; }
        public string NearestEndStop { get; set; }
        public double DistanceFromEndStop { get; set; }
        public JourneyRoute OptimalRoute { get; set; }
        public List<JourneyRoute> AlternativeRoutes { get; set; }
    }
}
