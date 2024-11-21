using System.ComponentModel.DataAnnotations;
using _3_Shared.Models.Entities;

namespace _2_Domain.Publication.Models.Entities;

public class ImageListModel : ModelBase
{
    [Key]
    [Required]
    [Range(
        0, int.MaxValue, 
        ErrorMessage = "'Id' must be a valid number."
    )]
    public int Id { get; init; }
    
    [Required]
    public int PublicationId { get; set; }
    
    [Required] public List<string> ImageList { get; set; }
}