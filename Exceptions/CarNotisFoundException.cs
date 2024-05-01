namespace RoadReady.Exceptions
{
    public class CarNotisFoundException : Exception
    {
        string message;
        public CarNotisFoundException()
        {
            message = "no car found. An error occurred while searching for cars.";
        }
        public string Message => message;

    }
}
