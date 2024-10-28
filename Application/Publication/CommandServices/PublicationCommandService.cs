using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Repositories;
using _2_Domain.Publication.Services;
using _3_Data;
using _3_Shared.Domain.Models;
using _3_Shared.Domain.Models.User;
using _3_Shared.Middleware.Exceptions;
using Domain.Publication.Models.ValueObjects;

namespace Application.Publication.CommandServices;

public class PublicationCommandService : IPublicationCommandService
{
    //  @Dependencies
    private readonly IPublicationRepository _publicationRepository;
    private readonly IUserManagerRepository _userManagerRepository;
    
    //  @Constructor
    public PublicationCommandService(
        IPublicationRepository publicationRepository,
        IUserManagerRepository userManagerRepository
    )
    {
        this._publicationRepository = publicationRepository;
        this._userManagerRepository = userManagerRepository;
    }
    
    //  @Methods
    public async Task<int> Handle(PublicationModel publication)
    {
        var result = await this._userManagerRepository.GetUserByIdAsync(publication.UserId);
        if (result == null)
        {
            throw new ArgumentException("User not found with this Id!");
        }
        
        //  @Validations
        //  1.  Users can't post more than 'UserConstraints.MaxNormalUserPublications' publications.
        //      An account upgrade is required for more publications.
        var userPublications = await this._publicationRepository.UserPublications(publication.UserId);
        if (
            (userPublications.Count >= (int)UserConstraints.MaxPublicationBasicUser) && 
            (result.Role == UserRole.BasicUser.ToString())
        )
        {
            throw new MaxPublicationLimitReachedException("User reached the maximum publication limit!");
        }
        
        //  2.  Priority is set by default using the value of [[UserConstraints.PublicationPriorityBasicUser]].
        if (result.Role == UserRole.PremiumUser.ToString())
        {
            publication.Priority = (double) UserConstraints.PublicationPriorityPremiumUser;
        }
        else
        {
            publication.Priority = (double) UserConstraints.PublicationPriorityBasicUser;
        }
        
        //  3.  Check if the publication type is valid
        if (!Enum.IsDefined(typeof(EPropertyType), publication.Type)) throw new ArgumentException("Invalid ServiceType");
        
        //  4.  Check if the publication operation is valid
        if (!Enum.IsDefined(typeof(EOperation), publication.Operation)) throw new ArgumentException("Invalid Operation");
        
        //  5.  Check if the publication sale state is valid
        var saleStateWithoutSpaces = publication.SaleState.Replace(" ", "");
        if (!Enum.IsDefined(typeof(ESaleState), saleStateWithoutSpaces)) throw new ArgumentException("Invalid SaleState");
                publication.SaleState = saleStateWithoutSpaces;
        
        //  6.  Check if the publication project stage is valid
        var projectStageWithoutSpaces = publication.ProjectStage.Replace(" ", "");
        if (!Enum.IsDefined(typeof(EProjectStage), projectStageWithoutSpaces)) throw new ArgumentException("Invalid ProjectStage");
        publication.ProjectStage = projectStageWithoutSpaces;
        
        return await this._publicationRepository.PostPublicationAsync(publication);
    }

    public async Task<int> Handle(int id)
    {
        if (id <= 0)
        {
            throw new InvalidIdException("Invalid Id!");
        }
        
        return await this._publicationRepository.DeletePublicationAsync(id);
    }
}