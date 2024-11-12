using _3_Data;
using Domain.IAM.Models.Commands;
using Domain.IAM.Services.Commands;
using Domain.Publication.Repositories;

namespace Application.IAM.CommandServices;

public class UserManagerCommandService : IUserManagerCommandService
{
    //  @Dependencies
    private readonly IUserManagerRepository _userManagerRepository;
    
    //  @Constructor
    public UserManagerCommandService(
        IUserManagerRepository userManagerRepository
    )
    {
        this._userManagerRepository = userManagerRepository;
    }
    
    //  @Methods
    public async Task<bool> UpdateUser(UpdateUserCommand command)
    {
        return await this._userManagerRepository.UpdateUser(command);
    }
}