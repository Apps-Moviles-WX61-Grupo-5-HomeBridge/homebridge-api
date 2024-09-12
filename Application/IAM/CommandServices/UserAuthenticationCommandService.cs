using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using _2_Domain.IAM.Models.Commands;
using _2_Domain.IAM.Models.ValueObjects;
using _2_Domain.IAM.Repositories;
using _2_Domain.IAM.Services;
using _2_Domain.IAM.Services.Commands;
using _3_Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.IAM.CommandServices;

public class UserAuthenticationCommandService : IUserAuthenticationCommandService
{
    //  @Dependencies
    private readonly IUserAuthenticationData _userAuthenticationData;
    private readonly IConfiguration _configuration;
    
    //  @Constructor
    public UserAuthenticationCommandService(
        IUserAuthenticationData userAuthenticationDomain, 
        IConfiguration configuration
    )
    {
        this._userAuthenticationData = userAuthenticationDomain;
        this._configuration = configuration;
    }
    
    //  @Methods
    private string GenerateToken(int userId)
    {
        var key = this._configuration.GetValue<string>("JwtSettings:key");
        var keyBytes = Encoding.ASCII.GetBytes(key);

        var claims = new ClaimsIdentity();
        claims.AddClaim(
            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString()
            )
        );

        var credentialsToken = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = credentialsToken
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

        string createdToken = tokenHandler.WriteToken(tokenConfig);
        return createdToken;
    }
    private string GenerateRefreshToken()
    {
        var byteArray = new byte[64];
        var refreshToken = "";
        
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(byteArray);
            refreshToken = Convert.ToBase64String(byteArray);
        }

        return refreshToken;
    }
    private async Task<AuthenticationResults> SaveRefreshTokenRecord(
        int userId,
        string token,
        string refreshToken
    )
    {
        var refreshTokenRecord = new RefreshTokenRecord
        {
            UserId = userId,
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddHours(3), //  !Change to a real scenario value.
            Created = DateTime.UtcNow
        };
        
        await this._userAuthenticationData.SaveRefreshTokenAsync(refreshTokenRecord);
        
        return new AuthenticationResults
        {
            token = token,
            refreshToken = refreshToken,
            result = true,
            message = "Refresh Token saved!"
        };
    }
    
    //  @Implementations
    public async Task<AuthenticationResults> Handle(CreateRefreshTokenCommand command)
    {
        var foundRefreshToken = await this._userAuthenticationData.FindExistingRefreshTokenAsync(command);

        if (foundRefreshToken == null)
        {
            return new AuthenticationResults()
            {
                result = false,
                message = "Invalid Refresh Token"
            };
        }
        
        var createdRefreshToken = GenerateRefreshToken();
        var createdToken = GenerateToken(command.UserId);

        return await this.SaveRefreshTokenRecord(command.UserId, createdToken, createdRefreshToken);
    }
}