namespace _2_Domain.Search.Models.Queries;

public class SearchQuery
{
    public string SearchInput { set; get; }
    public int Type { set; get; }
    public float PriceMin { set; get; }
    public float PriceMax { set; get; }
}