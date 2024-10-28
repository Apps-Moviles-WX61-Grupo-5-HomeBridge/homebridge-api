using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;

namespace _2_Domain.Publication.Services;

public interface IPublicationQueryService
{
     public Task<PublicationModel?> Handle(GetPublicationByIdQuery query);
     public Task<List<PublicationModel>> PublicationsByUserId(int userId);
     
     public Task<List<PublicationModel>> Publications(int amount);
     
     public Task<ImageListModel> ImageListByPublicationId(int publicationId);
}