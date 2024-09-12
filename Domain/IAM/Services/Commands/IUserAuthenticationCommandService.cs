using _2_Domain.IAM.Models.Commands;
using _2_Domain.IAM.Models.ValueObjects;

namespace _2_Domain.IAM.Services.Commands;

public interface IUserAuthenticationCommandService
{
    public Task<AuthenticationResults> Handle(CreateRefreshTokenCommand command);
}