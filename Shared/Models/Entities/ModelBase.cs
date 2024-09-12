namespace _3_Shared.Models.Entities;

public class ModelBase
{
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}