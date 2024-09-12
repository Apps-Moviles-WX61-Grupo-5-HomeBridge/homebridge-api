using _2_Domain.Publication.Models.Entities;
using _2_Domain.Search.Models.Queries;
using _2_Domain.Search.Repositories;
using _3_Data.Shared.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _3_Data.Search.Persistence;

public class SearchRepository : ISearchRepository
{
    //  @Dependencies
    private readonly PropertunityDataCenterContext _propertunityDataCenterContext;

    //  @Constructor
    public SearchRepository(
        PropertunityDataCenterContext propertunityDataCenterContext
    )
    {
        this._propertunityDataCenterContext = propertunityDataCenterContext;
    }

    //  @Methods
    public async Task<List<PublicationModel>> SearchAsync(SearchQuery search)
    {
        var result = await this._propertunityDataCenterContext.Publication.Where(u =>
                search.PriceMin <= u.Price &&
                u.Price <= search.PriceMax &&
                u._Location.Address.Contains(search.SearchInput) &&
                !u.IsDeleted)
            .ToListAsync();
        
        return result;
    }
}