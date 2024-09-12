using _2_Domain.IAM.Models.Commands;

namespace _2_Domain.IAM.Services.Commands;

public interface IUserRegistrationCommandService
{
    public Task<int> Handle(UserRegistrationCommand data);
}