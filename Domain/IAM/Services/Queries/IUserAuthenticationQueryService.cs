using _2_Domain.IAM.Models.Queries;
using _2_Domain.IAM.Models.ValueObjects;

namespace _2_Domain.IAM.Services.Queries;

public interface IUserAuthenticationQueryService
{
    public Task<AuthenticationResults> Handle(GetTokenQuery query);
}