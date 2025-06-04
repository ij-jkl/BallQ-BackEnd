namespace Application.Common.Pagination.Service;

public class PaginationService : IPaginationService
{
    public async Task<PaginationDto<TDestination>> PaginateAsync<TSource, TDestination>(
        IQueryable<TSource> query,
        int pageNumber,
        int pageSize,
        Func<TSource, TDestination> mapFunc,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 5;

        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var mappedItems = items.Select(mapFunc).ToList();

        return new PaginationDto<TDestination>
        {
            Items = mappedItems,
            Metadata = new PaginationData
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber
            }
        };
    }
}