using _2_Domain.IAM.Models.Entities;
using _3_Data.Shared.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _3_Data.IAM.Persistence;

public class UserRegisterData : IUserRegisterData
{
    //  @Dependencies
    private readonly SaleSquareDataCenterContext _saleSquareDataCenterContext;

    //  @Constructor
    public UserRegisterData(
        SaleSquareDataCenterContext saleSquareDataCenterContext
    )
    {
        this._saleSquareDataCenterContext = saleSquareDataCenterContext;
    }

    //  @Methods
    public async Task<int> CreateUserAsync(User data)
    {
        var executionStrategy = this._saleSquareDataCenterContext.Database.CreateExecutionStrategy();
        
        await executionStrategy.ExecuteAsync(async () =>
        {
            using (var transaction = await this._saleSquareDataCenterContext.Database.BeginTransactionAsync())
            {
                try
                {
                    data._UserInformation.IsDeleted = false;
                    this._saleSquareDataCenterContext.Users.Add(data);
                    await this._saleSquareDataCenterContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(exception.Message);
                }
            }        
        });
        
        return data.Id;
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var result = await this._saleSquareDataCenterContext.Users.
            Where(u => u._UserCredentials.Email == email)
            .FirstOrDefaultAsync();
        
        return result;
    }
}