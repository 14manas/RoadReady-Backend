namespace RoadReady.Exceptions
{
    public class CarSearchedNotFoundException : Exception
    {
       
            string message;
            public CarSearchedNotFoundException()
            {
                message = "no car deatils  found. An error occurred while searching for car deatils.";
            }
            public string Message => message;

       
    }
}
