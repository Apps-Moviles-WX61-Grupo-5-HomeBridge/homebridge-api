using _2_Domain.Publication.Models.Entities;

namespace _2_Domain.Publication.Services;

public interface IPublicationCommandService
{
    public Task<int> Handle(PublicationModel publication);

    public Task<int> Handle(int id);
}