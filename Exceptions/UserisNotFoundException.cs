namespace RoadReady.Exceptions
{
    public class UserisNotFoundException : Exception
    {
        string message;
        public UserisNotFoundException()
        {
            message = "no User found. An error occurred while searching for User.";
        }
        public string Message => message;

    }
}
