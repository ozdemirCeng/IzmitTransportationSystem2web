using System.Collections.Generic;

namespace IzmitTransportationSystem.Models
{
    public class Stop
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Coordinates Location { get; set; }
        public bool IsTerminal { get; set; }
        public List<NextStop> NextStops { get; set; }
        public Transfer Transfer { get; set; }

        public double DistanceTo(Coordinates location)
        {
            return Location.CalculateDistance(location);
        }
    }
}
