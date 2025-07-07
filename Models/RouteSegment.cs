namespace IzmitTransportationSystem.Models
{
    public class RouteSegment
    {
        public string FromStopId { get; set; }
        public string ToStopId { get; set; }
        public string VehicleType { get; set; }
        public bool IsTransfer { get; set; }
        public double Distance { get; set; }
        public int Duration { get; set; }
        public double Fare { get; set; }
    }
}
