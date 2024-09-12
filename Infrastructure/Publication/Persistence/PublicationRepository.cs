using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;
using _2_Domain.Publication.Repositories;
using _3_Data.Shared.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _3_Data.Publication.Persistence;

public class PublicationRepository : IPublicationRepository
{
    //  @Dependencies
    private readonly PropertunityDataCenterContext _propertunityDataCenterContext;

    //  @Constructor
    public PublicationRepository(
        PropertunityDataCenterContext propertunityDataCenterContext
    )
    {
        this._propertunityDataCenterContext = propertunityDataCenterContext;
    }

    //  @Methods
    public async Task<PublicationModel?> GetPublicationAsync(GetPublicationQuery query)
    {
        var result = await this._propertunityDataCenterContext.Publication.
            Where(u => 
                (u.Id == query.Id) && 
                (u.IsDeleted == false)
            ).FirstOrDefaultAsync();
        
        return result;
    }
    
    public async Task<int> PostPublicationAsync(PublicationModel publication)
    {
        var executionStrategy = this._propertunityDataCenterContext.Database.CreateExecutionStrategy();
        
        await executionStrategy.ExecuteAsync(async () =>
        {
            using (var transaction = await this._propertunityDataCenterContext.Database.BeginTransactionAsync())
            {
                try
                {
                    publication.IsDeleted = false;
                    this._propertunityDataCenterContext.Publication.Add(publication);
                    await this._propertunityDataCenterContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                }
            }        
        });
        
        return publication.Id;
    }

    public async Task<List<PublicationModel>> GetUserPublicationsAsync(int userId)
    {
        var result = await this._propertunityDataCenterContext.Publication.
            Where(u => 
                (u.UserId == userId) && 
                (u.IsDeleted == false)
            ).ToListAsync();

        return result;
    }

    public async Task<int> DeletePublicationAsync(int publicationId)
    {
        var publication = await this._propertunityDataCenterContext.Publication.
            Where(u => 
                (u.Id == publicationId) && 
                (u.IsDeleted == false)
            ).FirstOrDefaultAsync();

        if (publication == null)
        {
            return -1;
        }

        var executionStrategy = this._propertunityDataCenterContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            using (var transaction = await this._propertunityDataCenterContext.Database.BeginTransactionAsync())
            {
                try
                {
                    publication.IsDeleted = true;
                    await this._propertunityDataCenterContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                }
            }        
        });

        return publication.Id;
    }

    public async Task<int> MarkAsExpiredAsync(PublicationModel publication)
    {
        var executionStrategy = this._propertunityDataCenterContext.Database.CreateExecutionStrategy();
        
        await executionStrategy.ExecuteAsync(async () =>
        {
            using (var transaction = await this._propertunityDataCenterContext.Database.BeginTransactionAsync())
            {
                try
                {
                    publication.HasExpired = true;
                    await this._propertunityDataCenterContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                }
            }        
        });
        
        return publication.Id;
    }
}