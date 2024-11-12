using _2_Domain.IAM.Models.Entities;
using _3_Data.Shared.Contexts;
using Domain.IAM.Models.Commands;
using Microsoft.EntityFrameworkCore;

namespace _3_Data.IAM.Persistence;

public class UserManagerRepository : IUserManagerRepository
{
    //  @Dependencies
    private readonly SaleSquareDataCenterContext _salesquareDataCenterContext;

    //  @Constructor
    public UserManagerRepository(
        SaleSquareDataCenterContext salesquareDataCenterContext
    )
    {
        this._salesquareDataCenterContext = salesquareDataCenterContext;
    }

    //  @Methods
    public async Task<UserInformation?> GetUserByIdAsync(int id)
    {
        var result = await this._salesquareDataCenterContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        return result?._UserInformation;
    }
    public async Task<List<UserInformation>> GetUserByUsername(string username)
    {
        var result = await this._salesquareDataCenterContext.Users
            .Where(u => u._UserInformation.Name.Contains(username))
            .ToListAsync();
        
        var resultList = new List<UserInformation>();
        foreach (User user in result)
        {
            resultList.Add(user._UserInformation);
        }
        
        return resultList;
    }

    public async Task<bool> UpdateUser(UpdateUserCommand command)
    {
        var existingUser = await this.GetUserByIdAsync(command.UserId);
        if (existingUser == null) return false;
        
        existingUser.UserDescription = command.Description;
        existingUser.UserProfilePhotoUrl = command.PhotoUrl;
        existingUser.Name = command.Name;
        await this._salesquareDataCenterContext.SaveChangesAsync();
        return true;
    }
}