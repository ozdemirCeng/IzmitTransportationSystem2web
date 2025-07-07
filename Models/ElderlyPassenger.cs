namespace IzmitTransportationSystem.Models
{
    public class ElderlyPassenger : Passenger
    {
        public ElderlyPassenger()
        {
            PassengerType = "Elderly";
        }

        public override double CalculateDiscountFactor() => 0.25;
    }
}
