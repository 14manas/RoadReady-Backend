namespace RoadReady.DTO
{
    public class UserDTO
    {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime Dob { get; set; }
            //public UserTypeDTO? Usertype { get; set; }
           public int? Usertypeid { get; set; }
    }
    }
