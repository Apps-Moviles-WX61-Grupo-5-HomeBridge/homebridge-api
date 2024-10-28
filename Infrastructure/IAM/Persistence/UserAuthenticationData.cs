using _2_Domain.IAM.Models.Commands;
using _2_Domain.IAM.Models.Entities;
using _2_Domain.IAM.Models.Queries;
using _2_Domain.IAM.Models.ValueObjects;
using _2_Domain.IAM.Repositories;
using _3_Data.Shared.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _3_Data.IAM.Persistence;

public class UserAuthenticationData : IUserAuthenticationData
{
    //  @Dependencies
    private readonly SaleSquareDataCenterContext _saleSquareDataCenterContext;

    //  @Constructor
    public UserAuthenticationData(
        SaleSquareDataCenterContext saleSquareDataCenterContext
    )
    {
        this._saleSquareDataCenterContext = saleSquareDataCenterContext;
    }
    
    //  @Methods
    public async Task<User?> GetUserByEmailAsync(GetTokenQuery query)
    {
        var result = this._saleSquareDataCenterContext.Users.
            FirstOrDefaultAsync(user =>
            user._UserCredentials.Email == query.Email
        );

        return await result;
    }
    public async Task<RefreshTokenRecord?> FindExistingRefreshTokenAsync(CreateRefreshTokenCommand refreshToken)
    {
        var result = await this._saleSquareDataCenterContext.RefreshTokenRecords.
            FirstOrDefaultAsync(r =>
                r.Token == refreshToken.ExpiredToken &&
                r.RefreshToken == refreshToken.RefreshToken &&
                r.UserId == refreshToken.UserId
            );

        return result;
    }
    public async Task<int> SaveRefreshTokenAsync(RefreshTokenRecord refreshTokenRecord)
    {
        await this._saleSquareDataCenterContext.RefreshTokenRecords.AddAsync(refreshTokenRecord);
        await this._saleSquareDataCenterContext.SaveChangesAsync();

        return 1;
    }
}