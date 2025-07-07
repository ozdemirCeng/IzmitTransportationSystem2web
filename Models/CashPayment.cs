using System.Xml.Linq;

namespace IzmitTransportationSystem.Models
{
    public class CashPayment : PaymentMethod
    {
        public CashPayment(double amount)
        {
            Name = "Cash";
            Balance = amount;
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
