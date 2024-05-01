namespace RoadReady.Exceptions
{
    public class CityNotisFoundException : Exception
    {


        string message;
        public CityNotisFoundException()
        {
            message = "no city is found. An error occurred while searching for city.";
        }
        public string Message => message;


    }
}

