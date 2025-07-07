namespace IzmitTransportationSystem.Models
{
    public class JourneyRequest
    {
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public string PassengerType { get; set; }
        public PaymentInfo Payment { get; set; }
    }
}
