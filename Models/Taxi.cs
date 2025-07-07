namespace IzmitTransportationSystem.Models
{
    public class Taxi : Vehicle
    {
        private readonly double _openingFee;
        private readonly double _costPerKm;

        public Taxi(double openingFee, double costPerKm)
        {
            _openingFee = openingFee;
            _costPerKm = costPerKm;
        }

        public override string VehicleType => "Taxi";

        public override double CalculateFare(double distance, Passenger passenger)
        {
            // Takside yolcu indirimi uygulanmaz
            return _openingFee + (_costPerKm * distance);
        }

        public override int CalculateTravelTime(double distance)
        {
            return (int)(distance / 40.0 * 60);
        }
    }
}
