namespace AspNetAssignments.DTOs.Pagination;

public class ProductQueryParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value <= 0 ? 10 : value);
    }

    // Параметри пошуку та фільтрації
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    // Параметри сортування
    public string? SortBy { get; set; } = "CreatedDate";
    public bool IsSortDescending { get; set; } = true;
}
