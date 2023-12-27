namespace Survey.Shared.Models;

//[DataContract(Name = "data")]
public class LoginResponse
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }

    public string RefreshToken { get; set; }

}
