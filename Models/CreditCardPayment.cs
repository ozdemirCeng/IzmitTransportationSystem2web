using System.Xml.Linq;

namespace IzmitTransportationSystem.Models
{
    public class CreditCardPayment : PaymentMethod
    {
        public CreditCardPayment(double limit)
        {
            Name = "CreditCard";
            Balance = limit;
        }

        public override bool ProcessPayment(double amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }
    }
}
