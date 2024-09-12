namespace _2_Domain.IAM.Models.Queries;

public class GetTokenQuery
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}