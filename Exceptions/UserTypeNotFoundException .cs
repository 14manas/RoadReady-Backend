namespace RoadReady.Exceptions
{
    public class UserTypeisFoundException : Exception
    {
        string message;
        public UserTypeisFoundException()
        {
            message = "no UserType found. An error occurred while searching for UserType.";
        }
        public string Message => message;

    }
}
