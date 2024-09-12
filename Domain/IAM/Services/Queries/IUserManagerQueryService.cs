using _2_Domain.IAM.Models.Entities;
using _2_Domain.IAM.Models.Queries;

namespace _2_Domain.IAM.Services.Queries;

public interface IUserManagerQueryService
{
    public Task<UserInformation?> Handle(GetUserByIdQuery id);
}