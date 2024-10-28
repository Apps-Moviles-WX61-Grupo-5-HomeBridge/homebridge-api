namespace _2_Domain.IAM.Models.Queries;

public class GetTokenQuery
{
    public string Password { get; set; }
    public string Email { get; set; }
}