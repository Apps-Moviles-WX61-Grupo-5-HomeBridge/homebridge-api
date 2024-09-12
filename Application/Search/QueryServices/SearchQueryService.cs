using _2_Domain.Publication.Models.Entities;
using _2_Domain.Search.Models.Queries;
using _2_Domain.Search.Repositories;
using _2_Domain.Search.Services;
using _3_Shared.Domain.Models;

namespace Application.Search.QueryServices;

public class SearchQueryService : ISearchQueryService
{
    //  @Dependencies
    private readonly ISearchRepository _searchRepository; 
    
    //  @Constructor
    public SearchQueryService(
        ISearchRepository searchRepository
    )
    {
        this._searchRepository = searchRepository;
    }
    
    //  @Methods
    public async Task<List<PublicationModel>> Handle(SearchQuery search)
    {
        //  @Validations
        //  1.  'search.Type' must be a valid 'SearchConstraints' enum value.
        //      To avoid hardcoding, we can use Enum.IsDefined.
        if (!Enum.IsDefined(typeof(SearchConstraints), search.Type))
        {
            throw new Exception("Type appears to be invalid.");
        }
        
        //  2.  As a general rule, it doesn't matter if priceMin is higher than priceMax.
        //      It'll just pick the highest value and reorganize the range.
        //      After all, this is a model, not a request anymore.
        if (search.PriceMin > search.PriceMax)
        {
            (search.PriceMin, search.PriceMax) = (search.PriceMax, search.PriceMin);
        }

        var results = await this._searchRepository.SearchAsync(search);
        return await Task.FromResult(results);
    }
}