namespace IzmitTransportationSystem.Models
{
    public abstract class Vehicle
    {
        public abstract string VehicleType { get; }
        public abstract double CalculateFare(double distance, Passenger passenger);
        public abstract int CalculateTravelTime(double distance);
    }
}
