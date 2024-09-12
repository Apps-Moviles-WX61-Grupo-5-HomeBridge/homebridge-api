namespace _2_Domain.IAM.Models.ValueObjects;

public class RefreshTokenModel
{
    public string ExpiredToken { set; get; }
    public string RefreshToken { set; get; }
    public int UserId { set; get; }
}