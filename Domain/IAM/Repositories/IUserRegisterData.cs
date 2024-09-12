using _2_Domain.IAM.Models.Entities;

namespace _3_Data;

public interface IUserRegisterData
{
    Task<int> CreateUserAsync(User data);
    Task<User?> GetUserByEmailAsync(string email);
}