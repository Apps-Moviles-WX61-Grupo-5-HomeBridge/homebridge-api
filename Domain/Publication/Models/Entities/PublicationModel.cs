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
    
     [Required]
        public List<string> ImageList { get; set; }
    
        [Required] public float CoveredArea { get; set; }
    
        [Required] public float TotalArea { get; set; }
    
        [Required] public string Type { get; set; }
    
        [Required] public string Operation { get; set; }
    
        [Required] public string Delivery { get; set; }
    
        [Required] public int DormitoryQuantity { get; set; }
    
        [Required] public int BathroomQuantity { get; set; }
    
        [Required] public int ParkingLotQuantity { get; set; }
    
        [Required] public string SaleState { get; set; }
    
        [Required] public string ProjectStage { get; set; }
    
        [Required] public DateTime ProjectStartDate { get; set; }
    
        [Required] public int Antiquity { get; set; }
    
        public string Service { get; set; }
    
    public bool HasExpired { get; set; } = false;
    public DateTime ExpiresAt { get; set; }
}