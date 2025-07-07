namespace IzmitTransportationSystem.Models
{
    public class GeneralPassenger : Passenger
    {
        public GeneralPassenger()
        {
            PassengerType = "General";
        }

        public override double CalculateDiscountFactor() => 1.0;
    }
}
