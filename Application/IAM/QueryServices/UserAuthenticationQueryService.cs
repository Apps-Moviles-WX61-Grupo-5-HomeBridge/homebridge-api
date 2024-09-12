using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using _2_Domain.IAM.Models.Entities;
using _2_Domain.IAM.Models.Queries;
using _2_Domain.IAM.Models.ValueObjects;
using _2_Domain.IAM.Repositories;
using _2_Domain.IAM.Services;
using _2_Domain.IAM.Services.Commands;
using _2_Domain.IAM.Services.Queries;
using _3_Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.IAM.QueryServices;

public class UserAuthenticationQueryService : IUserAuthenticationQueryService
{
    //  @Dependencies
    private readonly IUserAuthenticationData _userAuthenticationData;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly IEncryptService _encryptService;
    
    //  @Constructor
    public UserAuthenticationQueryService(
        IUserAuthenticationData userAuthenticationDomain, 
        IConfiguration configuration,
        ITokenService tokenService,
        IEncryptService encryptService
    )
    {
        this._userAuthenticationData = userAuthenticationDomain;
        this._configuration = configuration;
        this._tokenService = tokenService;
        this._encryptService = encryptService;
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
            Expires = DateTime.UtcNow.AddHours(4),
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
            Expiration = DateTime.UtcNow.AddHours(4),
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
    public async Task<AuthenticationResults> Handle(GetTokenQuery query)
    {
        var existingUser = await _userAuthenticationData.GetUserByEmailAsync(query);
        if (existingUser == null)
        {
            throw new DataException("User doesn't exist!");
        }
        
        if (!_encryptService.Verify(query.Password, existingUser._UserCredentials.HashedPassword))
        {
            throw new DataException("Invalid password or username");
        }
        
        string createdToken = _tokenService.GenerateToken(existingUser);
        string createdRefreshToken = GenerateRefreshToken();
        
        return await this.SaveRefreshTokenRecord(
            existingUser.Id,
            createdToken,
            createdRefreshToken
        );
    }
}