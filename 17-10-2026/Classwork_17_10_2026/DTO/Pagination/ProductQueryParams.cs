namespace Classwork_17_10_2026.DTO.Pagination;

public class ProductQueryParams
{
    //Pagination parameters
    private int MaxPageSize = 50;
    private int _pageSize = 10;
    public int PageNumber { get; set; } = 1;
    public int PageSize { 
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    //Filtering parameters and searching parameters
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    //Sorting parameters
    public string? SortBy { get; set; } = "CreatedDate";
    public bool IsSortDescending { get; set; } = true;
}
