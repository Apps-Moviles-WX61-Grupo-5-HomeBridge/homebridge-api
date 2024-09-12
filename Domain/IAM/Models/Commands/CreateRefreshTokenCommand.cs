namespace _2_Domain.IAM.Models.Commands;

public class CreateRefreshTokenCommand
{
    public string ExpiredToken { set; get; }
    public string RefreshToken { set; get; }
    public int UserId { set; get; }
}