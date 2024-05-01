namespace RoadReady.Exceptions
{
    public class PaymentNotFoundException : Exception
    {
        string message;
        public PaymentNotFoundException()
        {
            message = "no Payment is found. An error occurred while searching for Payments .";
        }
        public string Message => message;


    }
}
