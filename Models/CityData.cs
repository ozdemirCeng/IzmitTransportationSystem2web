using System.Collections.Generic;

namespace IzmitTransportationSystem.Models
{
    public class CityData
    {
        public string City { get; set; }
        public TaxiInfo Taxi { get; set; }
        public List<Stop> Stops { get; set; }
    }
}
