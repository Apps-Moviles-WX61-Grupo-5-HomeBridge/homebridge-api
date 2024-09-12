namespace _2_Domain.IAM.Models.Commands;

public class UserRegistrationCommand
{
    public string Username { set; get; }
    public string Password { set; get; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}