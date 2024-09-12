using System.IdentityModel.Tokens.Jwt;
using _1_API.Response;
using _2_Domain;
using _2_Domain.IAM.Models.Commands;
using _2_Domain.IAM.Models.Queries;
using _2_Domain.IAM.Services;
using _2_Domain.IAM.Services.Commands;
using _2_Domain.IAM.Services.Queries;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1_API.IAM.Controllers;

[Route("api/v1/user/")]
[ApiController] 
public class UserAuthenticationController : ControllerBase
{
    //  @Dependencies
    private readonly IMapper _mapper;
    private readonly IUserAuthenticationQueryService _authenticationQueryService;
    private readonly IUserAuthenticationCommandService _authenticationCommandService;
    
    //  @Constructor
    public UserAuthenticationController(
        IMapper mapper,
        IUserAuthenticationQueryService authenticationQueryService,
        IUserAuthenticationCommandService authenticationCommandService
    )
    {
        this._mapper = mapper;
        this._authenticationQueryService = authenticationQueryService;
        this._authenticationCommandService = authenticationCommandService;
    }
    
    
    
    /// <summary>
    ///     Allows the user to log into the system providing a JWT token as authentication.
    /// </summary>
    /// <param name="request">Body request parameters that represents the input credentials of the user.</param>
    /// <returns>
    ///     Returns a <c>AuthenticationResults</c> object as a valid response to the user's request.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows the user to log into the system providing a JWT token and a refresh token as a valid authentication.
    ///     <para>You have to provide the user's credentials such as password and username.</para>
    ///     <para>Here is an overview of the parameters of <c>UserAuthenticationRequest</c>: </para>
    ///         <para> &#149; <b>Username</b>: The username of the user. </para>
    ///         <para> &#149; <b>Password</b>: The password. </para>
    ///     <para>Both user's credentials are needed to log the user in successfully.</para>
    ///     <br></br>
    ///     <para>What's actually returns is a bit more complex, but still manageable.</para>
    ///     <para>The type of the returned response (successful log in) is an instance of <c>AuthenticationResults</c>.</para>
    ///     <para><c>AuthenticationResults</c> has the following properties: </para>
    ///         <para> &#149; <b>token</b>: The token used for any request. </para>
    ///         <para> &#149; <b>refreshToken</b>: Allows the user to refresh their token. </para>
    ///         <para> &#149; <b>result</b>: Successful or unsucessful. </para>
    ///         <para> &#149; <b>message</b>: Just a message, nothing else. </para>
    /// </remarks>
    /// <response code="200">Returns <b>a valid session</b> for the user login.</response>
    /// <response code="500"><b>Something wrong</b> appears to be with your query.</response>
    /// <response code="401">You <b>didn't provide the correct credentials</b> for a successful login.</response>
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> UserLogin([FromBody] GetTokenQuery request)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest();
        }

        var result = await this._authenticationQueryService.Handle(request);
        
        return Ok(result);
    }

    
    
    /// <summary>
    ///     Refreshes the user's token for more request without re-logging in.
    /// </summary>
    /// <param name="command">Body request parameters that represents the refresh token and the expired token.</param>
    /// <returns>
    ///     Returns a <c>authorizationResponse</c> object as a valid response to the user's new token request.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows the user to refresh their token for more requests without re-logging in.
    ///     <para>The parameters of the request are the following: </para>
    ///         <para> &#149; <b>ExpiredToken</b>: The expired token. </para>
    ///         <para> &#149; <b>RefreshToken</b>: The returned refresh token given when first login. </para>
    ///     <para>The type of the returned response is an instance of <c>AuthenticationResults</c>.</para>
    ///     <para><c>AuthenticationResults</c> has the following properties: </para>
    ///         <para> &#149; <b>token</b>: The token used for any request. </para>
    ///         <para> &#149; <b>refreshToken</b>: Allows the user to refresh their token. </para>
    ///         <para> &#149; <b>result</b>: Successful or unsucessful. </para>
    ///         <para> &#149; <b>message</b>: Just a message, nothing else. </para>
    ///     <para>You can use <i>refreshToken</i> to request a new token using the expired <c>token</c>.</para>
    /// </remarks>
    /// <response code="200">Returns <b>another valid session</b> for the user.</response>
    /// <response code="500"><b>Something wrong</b> appears to be with your query.</response>
    /// <response code="401"><b>Token may not be expired</b>, or any <b>input was incorrect</b>.</response>
    [HttpPost]
    [Route("refreshToken")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtainRefreshToken([FromBody] RefreshTokenCommand command)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest();
        }
            
        var tokenHandler = new JwtSecurityTokenHandler();
        var receivedExpiredToken = tokenHandler.ReadJwtToken(command.ExpiredToken);
        if (receivedExpiredToken.ValidTo > DateTime.UtcNow)
        {
            return BadRequest(new AuthenticationResponse()
            {
                result = false,
                message = "Token is not expired"
            });
        }

        var userId = receivedExpiredToken.Claims.First(t =>
            t.Type == JwtRegisteredClaimNames.NameId
        ).Value.ToString();
        var refreshTokenModel = this._mapper.Map<RefreshTokenCommand, CreateRefreshTokenCommand>(command);
        refreshTokenModel.UserId = int.Parse(userId);
        var authorizationResponse = await this._authenticationCommandService.Handle(refreshTokenModel);
        
        if (authorizationResponse.result)
        {
            return Ok(authorizationResponse);
        }
        
        return BadRequest(authorizationResponse);
    }
}