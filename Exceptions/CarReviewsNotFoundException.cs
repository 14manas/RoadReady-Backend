namespace RoadReady.Exceptions
{
    public class CarReviewsisNotFoundException : Exception
    {
       
            string message;
            public CarReviewsisNotFoundException()
            {
                message = "no car deatils  found. An error occurred while searching for car deatils.";
            }
            public string Message => message;

       
    }
}
