using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;
using Domain.Publication.Models.Queries;

namespace Domain.Publication.Repositories;

public interface IPublicationRepository
{
    public Task<PublicationModel?> GetPublicationAsync(GetPublicationByIdQuery id);
    
    public Task<int> PostPublicationAsync(PublicationModel publication);
    
    public Task<List<PublicationModel>> UserPublications(int userId);
    
    public Task<List<PublicationModel>> Publications(GetPublicationQuery query, int amount);
    
    public Task<int> DeletePublicationAsync(int publicationId);
    
    public Task<int> MarkAsExpiredAsync(PublicationModel publication);
    
    public Task<ImageListModel?> ImageList(int publicationId);
    
    public Task<int> PostImageListAsync(ImageListModel imageList);
}