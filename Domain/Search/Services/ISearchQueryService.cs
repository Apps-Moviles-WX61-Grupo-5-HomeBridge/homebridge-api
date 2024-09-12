using _2_Domain.Publication.Models.Entities;
using _2_Domain.Search.Models.Queries;

namespace _2_Domain.Search.Services;

public interface ISearchQueryService
{
    public Task<List<PublicationModel>> Handle(SearchQuery search);
}