using _2_Domain.IAM.Models.Entities;
using _2_Domain.IAM.Models.Queries;
using _2_Domain.IAM.Services.Queries;
using _3_Data;
using Microsoft.IdentityModel.Tokens;

namespace Application.Search.QueryServices;

public class UserManagerQueryService : IUserManagerQueryService
{
    //  @Dependencies
    private readonly IUserManagerRepository _userManagerRepository;
    
    //  @Constructor
    public UserManagerQueryService(
        IUserManagerRepository userManagerRepository
    )
    {
        this._userManagerRepository = userManagerRepository;
    }
    
    //  @Methods
    public async Task<UserInformation?> Handle(GetUserByIdQuery query)
    {
        if (query.Id <= 0)
        {
            throw new Exception("UserId is invalid.");
        }
        
        //  Proceed with your action, human.
        return await this._userManagerRepository.GetUserByIdAsync(query.Id);
    }
    
    public async Task<List<UserInformation>> Handle(GetUserByUsernameQuery query)
    {
        if (query.Username.IsNullOrEmpty())
        {
            throw new Exception("Username is invalid.");
        }
        
        //  Proceed with your action, human.
        return await this._userManagerRepository.GetUserByUsername(query.Username);
    }
}