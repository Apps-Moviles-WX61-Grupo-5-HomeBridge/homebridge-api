using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;
using _3_Data;
using _3_Shared.Domain.Models.Publication;
using _3_Shared.Domain.Models.User;
using _3_Shared.Middleware.Exceptions;
using Domain.Publication.Models.Queries;
using Domain.Publication.Repositories;
using Domain.Publication.Services;

namespace Application.Publication.QueryServices;

public class PublicationQueryService : IPublicationQueryService
{
    //  @Dependencies
    private readonly IPublicationRepository _publicationRepository;
    private readonly IUserManagerRepository _userManagerRepository;
    
    //  @Constructor
    public PublicationQueryService(
        IPublicationRepository publicationRepository,
        IUserManagerRepository userManagerRepository
    ) 
    {
        this._publicationRepository = publicationRepository;
        this._userManagerRepository = userManagerRepository;
    }
    
    //  @Methods
    public async Task<PublicationModel?> Handle(GetPublicationByIdQuery query)
    {
        if (query.PublicationId <= 0)
        {
            throw new InvalidIdException("Invalid Id!");
        }
        
        var result = await this._publicationRepository.GetPublicationAsync(query);
        if (result == null)
        {
            throw new PublicationNotFoundException("Publication not found!");
        }
        
        //  @Validations
        //  1.  Check if the publication has expired, otherwise verify and continue.
        var user = await this._userManagerRepository.GetUserByIdAsync(result.UserId);
        if (((DateTime.Now - result.CreatedDate).TotalDays > (double) UserConstraints.TimeActiveInDaysBasicUser) &&
            (user.Role == UserRole.BasicUser.ToString()))
        {
            await this._publicationRepository.MarkAsExpiredAsync(result);
        }
        
        if (result.HasExpired)
        {
            throw new PublicationExpiredException("Publication not found!");
        }

        return result;
    }

    public async Task<List<PublicationModel>> PublicationsByUserId(int userId)
    {
        if (userId <= 0) throw new InvalidIdException("Invalid Id!");
        
        var result = await this._publicationRepository.UserPublications(userId);
        if (result == null) throw new PublicationNotFoundException("Publication not found!");

        foreach (var publication in result)
        {
            //  @Validations
            //  1.  Check if the publication has expired, otherwise verify and continue.
            var user = await this._userManagerRepository.GetUserByIdAsync(publication.UserId);
            if (((DateTime.Now - publication.CreatedDate).TotalDays > (double) UserConstraints.TimeActiveInDaysBasicUser) &&
                (user.Role == UserRole.BasicUser.ToString()))
            {
                await this._publicationRepository.MarkAsExpiredAsync(publication);
            }
        }

        return result;
    }

    public async Task<List<PublicationModel>> Publications(GetPublicationQuery query)
    {
        var amount = query.Amount;
        if (amount > (int) PublicationConstraints.MaxPublicationRequests) amount = (int) PublicationConstraints.MaxPublicationRequests;
        var result = await this._publicationRepository.Publications(query, amount);
        if (result == null) throw new PublicationNotFoundException("Publication not found!");

        foreach (var publication in result)
        {
            //  @Validations
            //  1.  Check if the publication has expired, otherwise verify and continue.
            var user = await this._userManagerRepository.GetUserByIdAsync(publication.UserId);
            if (((DateTime.Now - publication.CreatedDate).TotalDays > (double) UserConstraints.TimeActiveInDaysBasicUser) &&
                (user.Role == UserRole.BasicUser.ToString()))
            {
                await this._publicationRepository.MarkAsExpiredAsync(publication);
            }
        }

        return result;
    }
    
    public async Task<List<PublicationModel>> JustPublications()
    {
        var result = await this._publicationRepository.JustPublications();

        foreach (var publication in result)
        {
            //  @Validations
            //  1.  Check if the publication has expired, otherwise verify and continue.
            var user = await this._userManagerRepository.GetUserByIdAsync(publication.UserId);
            if (((DateTime.Now - publication.CreatedDate).TotalDays > (double) UserConstraints.TimeActiveInDaysBasicUser) &&
                (user.Role == UserRole.BasicUser.ToString()))
            {
                await this._publicationRepository.MarkAsExpiredAsync(publication);
            }
        }

        return result;
    }
    
    public async Task<ImageListModel> ImageListByPublicationId(int publicationId)
    {
        if (publicationId <= 0) throw new InvalidIdException("Invalid PublicationId!");
        
        var result = await this._publicationRepository.ImageList(publicationId);
        if (result == null) throw new ImageListNotFoundException("ImageList not found!");

        return result;
    }
}