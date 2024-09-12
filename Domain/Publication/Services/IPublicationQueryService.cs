using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;

namespace _2_Domain.Publication.Services;

public interface IPublicationQueryService
{
     public Task<PublicationModel?> Handle(GetPublicationQuery query);
}