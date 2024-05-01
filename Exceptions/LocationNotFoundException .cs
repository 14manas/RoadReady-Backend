namespace RoadReady.Exceptions
{
    public class LocationFoundException : Exception
    {


        string message;
        public LocationFoundException()
        {
            message = "no location is found. An error occurred while searching for location.";
        }
        public string Message => message;


    }
}

