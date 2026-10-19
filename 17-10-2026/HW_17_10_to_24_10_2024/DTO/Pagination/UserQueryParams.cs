namespace HW_17_10_to_24_10_2026.DTO.Pagination;

public class UserQueryParams
{
    //Pagination parameters
    private int MaxPageSize = 50;
    private int _pageSize = 10;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    //Filtering parameters and searching parameters
    public string? Search { get; set; }
    public string? Role { get; set; }

    //Sorting parameters
    public string? SortBy { get; set; } = "CreatedAt";
    public bool IsSortDescending { get; set; } = true;
}
