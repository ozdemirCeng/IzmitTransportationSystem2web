using System.Collections.Generic;

namespace IzmitTransportationSystem.Models
{
    public class JourneyRoute
    {
        public List<RouteSegment> Segments { get; set; }
        public double TotalDistance { get; set; }
        public int TotalDuration { get; set; }
        public double TotalFare { get; set; }
        public string RouteType { get; set; }

        public JourneyRoute()
        {
            Segments = new List<RouteSegment>();
            TotalDistance = 0;
            TotalDuration = 0;
            TotalFare = 0;
        }

        public void AddSegment(RouteSegment segment)
        {
            Segments.Add(segment);
            TotalDistance += segment.Distance;
            TotalDuration += segment.Duration;
            TotalFare += segment.Fare;
        }
    }
}
