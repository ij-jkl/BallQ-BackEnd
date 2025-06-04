namespace Application.Common.Pagination.Data;

public class PaginationData
{
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
}