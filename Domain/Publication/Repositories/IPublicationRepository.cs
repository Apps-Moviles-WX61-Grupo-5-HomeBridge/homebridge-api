using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;

namespace _2_Domain.Publication.Repositories;

public interface IPublicationRepository
{
    public Task<PublicationModel?> GetPublicationAsync(GetPublicationByIdQuery id);
    
    public Task<int> PostPublicationAsync(PublicationModel publication);
    
    public Task<List<PublicationModel>> UserPublications(int userId);
    
    public Task<int> DeletePublicationAsync(int publicationId);
    
    public Task<int> MarkAsExpiredAsync(PublicationModel publication);
}