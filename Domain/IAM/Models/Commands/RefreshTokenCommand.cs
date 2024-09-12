namespace _2_Domain.IAM.Models.Commands;

public class RefreshTokenCommand
{
    public string ExpiredToken { set; get; }
    public string RefreshToken { set; get; }
}