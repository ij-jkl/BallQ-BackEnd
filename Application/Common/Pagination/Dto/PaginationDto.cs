namespace Application.Common.Pagination.Dto;

public class PaginationDto<T>
{
    public List<T> Items { get; set; } = new();
    public PaginationData Metadata { get; set; } = new();
}