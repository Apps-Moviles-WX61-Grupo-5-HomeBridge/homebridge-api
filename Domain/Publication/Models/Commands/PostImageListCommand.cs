namespace _2_Domain.Publication.Models.Commands;

public class PostImageListCommand
{
    public int PublicationId { get; init; }
    
    public List<string> ImageList { get; set; }
}