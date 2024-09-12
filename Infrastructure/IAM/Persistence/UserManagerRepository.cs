using _2_Domain.IAM.Models.Entities;
using _3_Data.Shared.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _3_Data.IAM.Persistence;

public class UserManagerRepository : IUserManagerRepository
{
    //  @Dependencies
    private readonly PropertunityDataCenterContext _propertunityDataCenterContext;

    //  @Constructor
    public UserManagerRepository(
        PropertunityDataCenterContext propertunityDataCenterContext
    )
    {
        this._propertunityDataCenterContext = propertunityDataCenterContext;
    }

    //  @Methods
    public async Task<UserInformation?> GetUserByIdAsync(int id)
    {
        var result = await this._propertunityDataCenterContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        return result?._UserInformation;
    }
}