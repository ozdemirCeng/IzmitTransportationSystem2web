using System.Collections.Generic;

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
        
        // Koordinatlar
        public Coordinates StartLocation { get; set; }
        public Coordinates EndLocation { get; set; }
        public Coordinates StartStopLocation { get; set; }
        public Coordinates EndStopLocation { get; set; }

        // Segmentlerdeki durak koordinatlari (harita icin)
        public Dictionary<string, Coordinates> StopLocations { get; set; }
    }
}
