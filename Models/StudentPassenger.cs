namespace IzmitTransportationSystem.Models
{
    public class StudentPassenger : Passenger
    {
        public StudentPassenger()
        {
            PassengerType = "Student";
        }

        public override double CalculateDiscountFactor() => 0.5;
    }
}
