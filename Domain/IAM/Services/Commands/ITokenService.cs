using _2_Domain.IAM.Models.Entities;

namespace _2_Domain.IAM.Services.Commands;

public interface ITokenService
{
    string GenerateToken(User user);
    
    Task<int?> ValidateToken(string token);
}