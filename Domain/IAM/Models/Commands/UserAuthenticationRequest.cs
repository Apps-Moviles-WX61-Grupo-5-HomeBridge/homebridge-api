namespace _2_Domain.IAM.Models.Commands;

public class UserAuthenticationRequest
{
    public string Username { set; get; }
    public string Password { set; get; }
}