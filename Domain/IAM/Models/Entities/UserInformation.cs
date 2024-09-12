using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using _3_Shared.Domain.Models;
using _3_Shared.Domain.Models.User;
using _3_Shared.Models.Entities;

namespace _2_Domain.IAM.Models.Entities;

[ComplexType]
public class UserInformation : ModelBase
{
    [Required] 
    [MaxLength(15)]
    public string Name { get; set; }
    
    [MaxLength(30)]
    public string Lastname { get; set; }
    
    [MaxLength(9)]
    public string PhoneNumber { get; set; }
    
    public string Role { get; set; } = UserRole.BasicUser.ToString();
}