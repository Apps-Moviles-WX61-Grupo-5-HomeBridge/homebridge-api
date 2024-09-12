using _2_Domain.Publication.Models.Entities;
using _2_Domain.Search.Models.Entities;
using _2_Domain.Search.Models.Queries;

namespace _2_Domain.Search.Repositories;

public interface ISearchRepository
{
    public Task<List<PublicationModel>> SearchAsync(SearchQuery search);
}