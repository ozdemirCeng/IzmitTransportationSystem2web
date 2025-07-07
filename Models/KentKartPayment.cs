using System.Xml.Linq;

namespace IzmitTransportationSystem.Models
{
    public class KentKartPayment : PaymentMethod
    {
        public KentKartPayment(double balance)
        {
            Name = "KentKart";
            Balance = balance;
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
