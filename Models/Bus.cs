namespace IzmitTransportationSystem.Models
{
    public class Bus : Vehicle
    {
        public override string VehicleType => "Bus";

        public override double CalculateFare(double distance, Passenger passenger)
        {
            double baseFare = 3.0;
            return baseFare * passenger.CalculateDiscountFactor();
        }

        public override int CalculateTravelTime(double distance)
        {
            return (int)(distance / 20.0 * 60);
        }
    }
}
