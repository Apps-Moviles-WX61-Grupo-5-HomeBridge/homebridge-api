using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;
using _2_Domain.Publication.Repositories;
using _2_Domain.Publication.Services;
using _3_Data;
using _3_Shared.Domain.Models.User;
using _3_Shared.Middleware.Exceptions;

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
    public async Task<PublicationModel?> Handle(GetPublicationQuery query)
    {
        if (query.Id <= 0)
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
}