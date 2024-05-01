namespace RoadReady.Exceptions
{
    public class CarDetailsisNotFoundException : Exception
    {
       
            string message;
            public CarDetailsisNotFoundException()
            {
                message = "no car deatils  found. An error occurred while searching for car deatils.";
            }
            public string Message => message;

       
    }
}
