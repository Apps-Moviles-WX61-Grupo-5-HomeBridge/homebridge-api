using _2_Domain.Publication.Models.Entities;
using _2_Domain.Publication.Models.Queries;
using _3_Data.Shared.Contexts;
using _3_Shared.Domain.Models.Publication;
using Domain.Publication.Models.Queries;
using Domain.Publication.Repositories;
using Microsoft.EntityFrameworkCore;

namespace _3_Data.Publication.Persistence;

public class PublicationRepository : IPublicationRepository
{
    //  @Dependencies
    private readonly SaleSquareDataCenterContext _saleSquareDataCenterContext;

    //  @Constructor
    public PublicationRepository(
        SaleSquareDataCenterContext saleSquareDataCenterContext
    )
    {
        this._saleSquareDataCenterContext = saleSquareDataCenterContext;
    }

    //  @Methods
    public async Task<PublicationModel?> GetPublicationAsync(GetPublicationByIdQuery query)
    {
        var result = await this._saleSquareDataCenterContext.Publication.
            Where(u => 
                (u.Id == query.PublicationId) && 
                (u.IsDeleted == false)
            ).FirstOrDefaultAsync();
        
        return result;
    }
    
    public async Task<int> PostPublicationAsync(PublicationModel publication)
    {
        var executionStrategy = this._saleSquareDataCenterContext.Database.CreateExecutionStrategy();
        
        await executionStrategy.ExecuteAsync(async () =>
        {
            using (var transaction = await this._saleSquareDataCenterContext.Database.BeginTransactionAsync())
            {
                try
                {
                    publication.IsDeleted = false;
                    this._saleSquareDataCenterContext.Publication.Add(publication);
                    await this._saleSquareDataCenterContext.SaveChangesAsync();
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

    public async Task<List<PublicationModel>> UserPublications(int userId)
    {
        var result = await this._saleSquareDataCenterContext.Publication.
            Where(u => 
                (u.UserId == userId) && 
                (u.IsDeleted == false)
            ).ToListAsync();

        return result;
    }

    public async Task<int> DeletePublicationAsync(int publicationId)
    {
        var publication = await this._saleSquareDataCenterContext.Publication.
            Where(u => 
                (u.Id == publicationId) && 
                (u.IsDeleted == false)
            ).FirstOrDefaultAsync();

        if (publication == null)
        {
            return -1;
        }

        var executionStrategy = this._saleSquareDataCenterContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            using (var transaction = await this._saleSquareDataCenterContext.Database.BeginTransactionAsync())
            {
                try
                {
                    publication.IsDeleted = true;
                    await this._saleSquareDataCenterContext.SaveChangesAsync();
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
        var executionStrategy = this._saleSquareDataCenterContext.Database.CreateExecutionStrategy();
        
        await executionStrategy.ExecuteAsync(async () =>
        {
            using (var transaction = await this._saleSquareDataCenterContext.Database.BeginTransactionAsync())
            {
                try
                {
                    publication.HasExpired = true;
                    await this._saleSquareDataCenterContext.SaveChangesAsync();
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

    public async Task<ImageListModel?> ImageList(int publicationId)
    {
        var result = await this._saleSquareDataCenterContext.ImageList.
            Where(u => 
                (u.PublicationId == publicationId) && 
                (u.IsDeleted == false)
            ).FirstOrDefaultAsync();
        
        return result;
    }

    public async Task<int> PostImageListAsync(ImageListModel imageList)
    {
        var executionStrategy = this._saleSquareDataCenterContext.Database.CreateExecutionStrategy();
        
        await executionStrategy.ExecuteAsync(async () =>
        {
            using (var transaction = await this._saleSquareDataCenterContext.Database.BeginTransactionAsync())
            {
                try
                {
                    imageList.IsDeleted = false;
                    this._saleSquareDataCenterContext.ImageList.Add(imageList);
                    await this._saleSquareDataCenterContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                }
            }        
        });
        
        return imageList.Id;
    }

    public async Task<List<PublicationModel>> Publications(GetPublicationQuery query, int amount)
    {
        var result = await this._saleSquareDataCenterContext.Publication
            .Where(u => 
                (u.IsDeleted == false) &&
                (u._Location.Address.Contains(query.Location) ||
                 (u.Operation == query.OperationType) ||
                 (u.PlaceType == query.PlaceType)) ||
                ((query.PriceFrom <= u.Price) && (u.Price <= query.PriceTo)) || 
                (query.Bathrooms <= u.Bathrooms) ||
                (query.Garages <= u.Garages) || 
                (query.Rooms <= u.Rooms) ||
                ((query.AreaFrom <= u.Size) && (u.Size <= query.AreaTo))
            )
            .Take(amount)
            .ToListAsync();
        
        return result;
    }
    
    public async Task<List<PublicationModel>> JustPublications()
    {
        var result = await this._saleSquareDataCenterContext.Publication
            .Take((int) PublicationConstraints.MaxPublicationRequests)
            .ToListAsync();
        
        return result;
    }
}