namespace Domain.Publication.Models.Queries;

public class GetPublicationQuery
{
    public string Location { set; get; } = string.Empty;
    public int OperationType { set; get; }
    public int PlaceType { set; get; }
    public float PriceFrom { set; get; }
    public float PriceTo { set; get; }
    public int Rooms { set; get; }
    public int Bathrooms { set; get; }
    public int Garages { set; get; }
    public float AreaFrom { set; get; }
    public float AreaTo { set; get; }
}