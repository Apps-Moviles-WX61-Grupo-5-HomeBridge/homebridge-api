using System.ComponentModel.DataAnnotations;

namespace _2_Domain.IAM.Models.ValueObjects;

public class RefreshTokenRecord
{
    [Key]
    public int Id { get; set; }
    
    public int? UserId { get; set; }
    
    [Required]
    public string Token { get; set; }
    [Required]
    public string RefreshToken { get; set; }
    
    public DateTime? Expiration { get; set; }
    
    public DateTime? Created { get; set; }
    
    public bool? IsActive { get; set; }
}