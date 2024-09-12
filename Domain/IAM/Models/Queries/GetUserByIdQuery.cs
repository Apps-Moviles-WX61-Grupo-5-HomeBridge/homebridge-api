namespace _2_Domain.IAM.Models.Queries;

public class GetUserByIdQuery
{
    public int Id { get; set; }
    
    public GetUserByIdQuery(int id)
    {
        Id = id;
    }
}