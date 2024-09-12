using System.ComponentModel.DataAnnotations;

namespace _2_Domain.IAM.Models.Entities;

public class User
{
    [Key]
    public int Id { get; init; }
    
    public UserCredentials _UserCredentials { get; init; }
    public UserInformation _UserInformation { get; init; }
}