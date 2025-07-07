namespace IzmitTransportationSystem.Models
{
    public abstract class PaymentMethod
    {
        public string Name { get; protected set; }
        public double Balance { get; set; }
        public abstract bool ProcessPayment(double amount);
    }
}
