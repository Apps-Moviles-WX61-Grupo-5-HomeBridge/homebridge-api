using System.ComponentModel.DataAnnotations.Schema;

namespace _2_Domain.IAM.Models.Entities;

[ComplexType]
public class UserCredentials
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public string Email { get; set; }
}