using _1_API.Filters;
using _2_Domain;
using _2_Domain.IAM.Models.Queries;
using _2_Domain.IAM.Services.Commands;
using _2_Domain.IAM.Services.Queries;
using _3_Shared.Middleware.Exceptions;
using AutoMapper;
using Domain.IAM.Models.Commands;
using Domain.IAM.Models.Queries;
using Domain.IAM.Services.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1_API.IAM.Controllers;

[Route("api/v1/user/")]
[ApiController]
public class UserManagerController : ControllerBase
{
    //  @Dependencies
    private readonly IMapper _mapper;
    private readonly IUserManagerCommandService _userManagerCommandService;
    private readonly IUserManagerQueryService _userManagerQueryService;

    //  @Constructor
    public UserManagerController(
        IMapper mapper,
        IUserManagerCommandService userManagerCommandService,
        IUserManagerQueryService userManagerQueryService
    )
    {
        this._mapper = mapper;
        this._userManagerCommandService = userManagerCommandService;
        this._userManagerQueryService = userManagerQueryService;
    }
    
    
    
    /// <summary>
    ///     Obtain the information of a user by its ID.
    /// </summary>
    /// <returns>
    ///     Returns the information of a user of type <c>UserInformation</c>.
    /// </returns>
    /// <remarks>
    ///     This endpoint returs the information of a user of type <c>UserInformation</c>.
    ///     <para>If you were expecting to return also the user credentials, well not obviously.</para>
    ///     <para>You only need to provide an Id to start searching.</para>
    ///     <para>Note that this controller must be set with the annotation <i>[Authorize]</i></para>
    ///     <para>so any request to get the information of any user (at least public information)</para>
    ///     <para>must be authorized through login. But for this purpose, this is set as public and</para>
    ///     <para>anyone can access to this controller without authorization.</para>
    /// </remarks>
    /// <response code="200">Returns <b>the information of the user</b>.</response>
    /// <response code="404">User <b>not found</b>.</response>
    /// <response code="500"><b>Something went wrong</b>. Have you tried to unplug the internet cable?</response>
    [HttpGet]
    [Route("findById")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserInformation([FromQuery] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await this._userManagerQueryService.Handle(new GetUserByIdQuery(id));

        if (result == null)
        {
            throw new UserNotFoundException("User not found with that ID.");
        }

        return Ok(result);
    }

    
    
    /// <summary>
    ///     Obtain the information of a user by its username. Your input should match the username.
    /// </summary>
    /// <returns>
    ///     Returns the information of a user of type <c>UserInformation</c>.
    /// </returns>
    /// <remarks>
    ///     This endpoint returns the information of a user of type <c>UserInformation</c>.
    ///     <para>If you were expecting to return also the user credentials, well not obviously.</para>
    ///     <para>You only need to provide a username to start searching.</para>
    /// </remarks>
    /// <response code="200">Returns <b>the information of the user</b>.</response>
    /// <response code="404">User <b>not found</b>.</response>
    /// <response code="500"><b>Something went wrong</b>. Have you tried to unplug the internet cable?</response>
    [HttpGet]
    [Route("findByUsername")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserByUsername([FromQuery] GetUserByUsernameQuery query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await this._userManagerQueryService.Handle(query);
        
        return Ok(result);
    }

    [HttpPut]
    [Route("updateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await this._userManagerCommandService.UpdateUser(command);

        return Ok(result);
    }
}