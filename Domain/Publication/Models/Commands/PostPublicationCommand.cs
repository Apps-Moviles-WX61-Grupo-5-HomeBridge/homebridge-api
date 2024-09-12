namespace _2_Domain.Publication.Models.Commands;

public class PostPublicationCommand
{
    public string Title { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public string _Location_Address { get; set; }
    public int UserId { get; set; }
}