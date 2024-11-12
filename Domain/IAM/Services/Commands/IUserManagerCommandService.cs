using Domain.IAM.Models.Commands;

namespace Domain.IAM.Services.Commands;

public interface IUserManagerCommandService
{
    public Task<bool> UpdateUser(UpdateUserCommand command);
}