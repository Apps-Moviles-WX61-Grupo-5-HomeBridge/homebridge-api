using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;
using Domain.Publication.Models.Queries;

namespace Domain.Publication.Services;

public interface IPublicationQueryService
{
     public Task<PublicationModel?> Handle(GetPublicationByIdQuery query);
     public Task<List<PublicationModel>> PublicationsByUserId(int userId);
     
     public Task<List<PublicationModel>> Publications(GetPublicationQuery query);
     
     public Task<List<PublicationModel>> JustPublications();
     
     public Task<ImageListModel> ImageListByPublicationId(int publicationId);
}