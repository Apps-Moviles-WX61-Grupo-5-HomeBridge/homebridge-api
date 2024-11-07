using _1_API.Filters;
using _2_Domain;
using _2_Domain.Publication.Models.Commands;
using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;
using _2_Domain.Publication.Services;
using AutoMapper;
using Domain.Publication.Models.Queries;
using Domain.Publication.Services;
using Microsoft.AspNetCore.Mvc;

namespace _1_API.Publication.Controllers;

[Route("api/v1/publication/")]
[ApiController]
public class PublicationController : ControllerBase
{
    //  @Dependencies
    private readonly IMapper _mapper;
    private readonly IPublicationCommandService _publicationCommandService;
    private readonly IPublicationQueryService _publicationQueryService;

    //  @Constructor
    public PublicationController(
        IMapper mapper,
        IPublicationCommandService publicationCommandService,
        IPublicationQueryService publicationQueryService
    )
    {
        this._mapper = mapper;
        this._publicationCommandService = publicationCommandService;
        this._publicationQueryService = publicationQueryService;
    }
    
    
    
    /// <summary>
    ///     Search for publications but given a valid ID (and a state).
    /// </summary>
    /// <param name="publicationGetListQuery">Query request parameter that represents what publication to retrieve.</param>
    /// <returns>
    ///     Returns a single publication that matches the query parameters.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows the front-end to retrieve a publication given an Id (and a state).
    ///     <para>The type of this parameter is an instance of <c>GetPublicationRequest</c>.</para>
    ///     <para><c>GetPublicationRequest</c> has the following properties: </para>
    ///         <para> &#149; <b>Id</b>: A valid Id of any publication. </para>
    ///         <para> &#149; <b>IsActive</b>: Optional; Checks if the publication is active (visible) or not (hidden). </para>
    ///     <para>Note that Id must be greater than 0</para>
    /// </remarks>
    /// <response code="200">Returns <b>a matched publication</b> corresponding to the query parameters.</response>
    /// <response code="500"><b>Something wrong</b> appears to be with your query.</response>
    /// <response code="404">Returns <b>nothing (null)</b> as no match was found.</response>
    [HttpGet]
    [Route("publicationById")]
    public async Task<IActionResult> GetPublicationById([FromQuery] GetPublicationByIdQuery publicationGetListQuery)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await this._publicationQueryService.Handle(publicationGetListQuery);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpGet]
    [Route("userPublications")]
    public async Task<IActionResult> GetPublicationsByUserId([FromQuery] int userId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await this._publicationQueryService.PublicationsByUserId(userId);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpGet]
    [Route("publications")]
    public async Task<IActionResult> GetPublications([FromQuery] GetPublicationQuery query)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var result = await this._publicationQueryService.Publications(query);

        if (result == null) return NotFound();

        return Ok(result);
    }
    
    [HttpGet]
    [Route("justPublications")]
    public async Task<IActionResult> GetJustPublications()
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var result = await this._publicationQueryService.JustPublications();

        if (result == null) return NotFound();

        return Ok(result);
    }
    
    [HttpGet]
    [Route("imageList")]
    public async Task<IActionResult> GetImageListByPublicationId([FromQuery] int amount)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await this._publicationQueryService.ImageListByPublicationId(amount);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    /// <summary>
    ///     Posts (creates) a publication based on your post parameters.
    /// </summary>
    /// <param name="publicationPostCommand">Body request parameters that represents a publication instance.</param>
    /// <returns>
    ///     Returns a valid id of the publication that was posted.
    /// </returns>
    /// <remarks>
    ///     Post (create) a new publication based on the parameters provided.
    ///     <para>The type of this parameter is an instance of <c>PostPublicationRequest</c>.</para>
    ///     <para><c>PostPublicationRequest</c> has the following properties: </para>
    ///         <para> &#149; <b>Title</b>: A title to be displayed. </para>
    ///         <para> &#149; <b>Description</b>: A description of your publication; make sure to provide a rich description. </para>
    ///         <para> &#149; <b>Price</b>: Price of the real state. </para>
    ///         <para> &#149; <b>_Location_Address</b>: Adress of your real state. </para>
    ///         <para> &#149; <b>UserId</b>: Who is posting this publication? Provide a valid id. </para>
    ///     <para>Note that UserId should be completed automatically because the user have already logged in.</para>
    /// </remarks>
    /// <response code="200">Returns <b>a valid id</b> of the publication that was posted.</response>
    /// <response code="500"><b>Something wrong</b> appears to be with your query.</response>
    /// <response code="400">Your publication <b>couldn't be created</b>; bad request.</response>
    //  [Authorize]
    [HttpPost]
    [Route("postPublication")]
    [CustomAuthorize("BasicUser", "PremiumUser")]
    public async Task<IActionResult> PostPublication([FromBody] PostPublicationCommand publicationPostCommand)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var publicationModel = this._mapper.Map<PublicationModel>(publicationPostCommand);
        var result = await this._publicationCommandService.Handle(publicationModel);

        if (result <= 0)
        {
            return BadRequest("Publication could not be posted as an invalid ID was returned.");
        }

        return Ok(result);
    }
    
    /// <summary>
    ///     Posts (creates) a imageList based on your post parameters.
    /// </summary>
    /// <param name="imageListPostCommand">Body request parameters that represents a imageList instance.</param>
    /// <returns>
    ///     Returns a valid id of the imageList that was posted.
    /// </returns>
    /// <remarks>
    ///     Post (create) a new imageList based on the parameters provided.
    ///     <para>The type of this parameter is an instance of <c>PostImageListRequest</c>.</para>
    ///     <para><c>PostImageListRequest</c> has the following properties: </para>
    ///         <para> &#149; <b>PublicationId</b>: Which publication does this imageList belongs to? Provide a valid id. </para>
    ///         <para> &#149; <b>ImageList</b>: A list of images url. </para>
    ///     <para>Note that UserId should be completed automatically because the user have already logged in.</para>
    /// </remarks>
    /// <response code="200">Returns <b>a valid id</b> of the imageList that was posted.</response>
    /// <response code="500"><b>Something wrong</b> appears to be with your query.</response>
    /// <response code="400">Your imageList <b>couldn't be created</b>; bad request.</response>
    //  [Authorize]
    [HttpPost]
    [Route("postImageList")]
    public async Task<IActionResult> PostImageList([FromBody] PostImageListCommand imageListCommand)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var imagesListModel = this._mapper.Map<ImageListModel>(imageListCommand);
        var result = await this._publicationCommandService.Handle(imagesListModel);

        if (result <= 0)
        {
            return BadRequest("ImageList could not be posted as an invalid ID was returned.");
        }

        return Ok(result);
    }

    /// <summary>
    ///     Softly deletes a publication based on the provided ID.
    /// </summary>
    /// <param name="id">Id is just required to delete the required publication.</param>
    /// <returns>
    ///     Returns '1' as a successfull deletion.
    /// </returns>
    /// <remarks>
    ///     <i>Softly deletes</i> a publication setting its state to inactive.
    ///     <para>If a publication is <i>inactive for too long</i>, then it will get permanently deleted.</para>
    /// </remarks>
    /// <response code="200">Returns <b>1</b> if the publication was successfully deleted.</response>
    /// <response code="500"><b>Something wrong</b> appears to be with your query.</response>
    /// <response code="400">Your publication <b>couldn't be deleted</b>; bad request.</response>
    [CustomAuthorize("BasicUser", "PremiumUser", "Admin")]
    [HttpDelete]
    [Route("deletePublication")]
    public async Task<IActionResult> DeletePublication([FromBody] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await this._publicationCommandService.Handle(id);

        if (result <= 0)
        {
            return BadRequest("Publication could not be deleted as an invalid ID was returned.");
        }

        return Ok(result);
    }
}