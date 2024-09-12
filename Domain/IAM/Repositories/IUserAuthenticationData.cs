using _2_Domain.IAM.Models.Commands;
using _2_Domain.IAM.Models.Entities;
using _2_Domain.IAM.Models.Queries;
using _2_Domain.IAM.Models.ValueObjects;

namespace _2_Domain.IAM.Repositories;

public interface IUserAuthenticationData
{
    public Task<User?> GetUserByEmailAsync(GetTokenQuery query);
    public Task<RefreshTokenRecord?> FindExistingRefreshTokenAsync(CreateRefreshTokenCommand command);
    public Task<int> SaveRefreshTokenAsync(RefreshTokenRecord refreshTokenRecord);
}