namespace IzmitTransportationSystem.Models
{
    public abstract class Passenger
    {
        public string PassengerType { get; protected set; }
        public virtual double CalculateDiscountFactor() => 1.0;
    }
}
