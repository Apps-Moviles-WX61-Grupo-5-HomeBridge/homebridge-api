using System.ComponentModel.DataAnnotations;
using _3_Shared.Domain.Models;
using _3_Shared.Domain.Models.Publication;
using _3_Shared.Domain.Models.User;
using _3_Shared.Models.Entities;

namespace _2_Domain.Publication.Models.Entities;

public class PublicationModel : ModelBase
{
    [Key]
    [Required]
    [Range(
        0, int.MaxValue, 
        ErrorMessage = "'Id' must be a valid number."
    )]
    public int Id { get; init; }
    
    [Required]
    public int UserId { get; init; }
    
    [Required]
    [StringLength(
        (int) PublicationConstraints.MaxTitleLength, 
        MinimumLength = (int) PublicationConstraints.MinTitleLength, 
        ErrorMessage = "'Title' must have a valid string length range."
    )]
    public string Title { get; set; }
    
    [Required]
    [StringLength(
        (int) PublicationConstraints.MaxDescriptionLength, 
        MinimumLength = (int) PublicationConstraints.MinDescriptionLength, 
        ErrorMessage = "'Description' must have a valid string length range."
    )]
    public string Description { get; set; }
    
    [Required]
    [Range(
        (int) PublicationConstraints.MinPrice, 
        double.MaxValue, 
        ErrorMessage = "'Price' can't be negative."
    )]
    public float Price { get; set; }
    
    [Required]
    public LocationModel _Location { get; set; }

    [Required] public double Priority { get; set; } = (double)UserConstraints.PublicationPriorityPremiumUser;
    
    public bool HasExpired { get; set; } = false;
    public DateTime ExpiresAt { get; set; }
}