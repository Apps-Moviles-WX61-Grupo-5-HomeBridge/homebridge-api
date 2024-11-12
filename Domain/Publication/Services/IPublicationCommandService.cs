using _2_Domain.Publication.Models.Entities;
using Domain.IAM.Models.Commands;
using Domain.Publication.Models.Commands;

namespace Domain.Publication.Services;

public interface IPublicationCommandService
{
    public Task<int> Handle(PublicationModel publication);

    public Task<int> Handle(int id);
    
    public Task<int> Handle(ImageListModel imageList);

    public Task<bool> UpdatePublication(UpdatePublicationCommand command);

    public Task<bool> UpdateImageList(UpdateImageListCommand command);
}