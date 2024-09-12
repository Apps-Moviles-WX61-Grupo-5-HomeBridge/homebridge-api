namespace _2_Domain.IAM.Models.ValueObjects;

public class AuthenticationResults
{
    public string token { get; set; }
    public string refreshToken { get; set; }
    public bool result { get; set; }
    public string message { get; set; }
}