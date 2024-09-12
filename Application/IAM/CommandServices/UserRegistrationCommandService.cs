using _2_Domain.IAM.Models.Commands;
using _2_Domain.IAM.Models.Entities;
using _2_Domain.IAM.Services.Commands;
using _3_Data;
using _3_Shared.Domain.Models.User;
using _3_Shared.Middleware.Exceptions;
using AutoMapper;

namespace Application.IAM.CommandServices;

public class UserRegistrationCommandService : IUserRegistrationCommandService
{
    //   @Dependencies  
    private readonly IMapper _mapper;
    private readonly IUserRegisterData _userRegisterData;
    private readonly ITokenService _tokenService;
    private readonly IEncryptService _encryptService;

    //   @Constructor
    public UserRegistrationCommandService(
        IMapper mapper,
        IUserRegisterData userRegisterData,
        ITokenService tokenService,
        IEncryptService encryptService
    )
    {
        this._mapper = mapper;
        this._userRegisterData = userRegisterData;
        this._tokenService = tokenService;
        this._encryptService = encryptService;
    }
   
    //   @Implementations
    public async Task<int> Handle(UserRegistrationCommand command)
    {
        var user = this._mapper.Map<UserRegistrationCommand, User>(command);
        
        //  @Validations
        //  1.  A user can't register if it already exists.
        //      The email must be unique while.
        var result = await this._userRegisterData.GetUserByEmailAsync(user._UserCredentials.Email);
        if (result != null)
        {
            throw new UserAlreadyExistsException("User already exists.");
        }

        user._UserInformation.Role = UserRole.BasicUser.ToString();
        user._UserCredentials.HashedPassword = _encryptService.Encrypt(command.Password);
        
        return await this._userRegisterData.CreateUserAsync(user);
    }
}