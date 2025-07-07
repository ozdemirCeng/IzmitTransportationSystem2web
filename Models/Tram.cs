namespace IzmitTransportationSystem.Models
{
    public class Tram : Vehicle
    {
        public override string VehicleType => "Tram";

        public override double CalculateFare(double distance, Passenger passenger)
        {
            double baseFare = 2.5;
            return baseFare * passenger.CalculateDiscountFactor();
        }

        public override int CalculateTravelTime(double distance)
        {
            return (int)(distance / 25.0 * 60);
        }
    }
}
