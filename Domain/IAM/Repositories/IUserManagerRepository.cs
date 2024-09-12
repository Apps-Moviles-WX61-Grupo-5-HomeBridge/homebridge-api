using _2_Domain.IAM.Models.Entities;

namespace _3_Data;

public interface IUserManagerRepository
{
    public Task<UserInformation?> GetUserByIdAsync(int id);
}