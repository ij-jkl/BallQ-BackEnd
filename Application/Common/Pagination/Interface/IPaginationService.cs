namespace Application.Common.Pagination.Interface;

public interface IPaginationService
{
    Task<PaginationDto<TDestination>> PaginateAsync<TSource, TDestination>(
        IQueryable<TSource> query,
        int pageNumber,
        int pageSize,
        Func<TSource, TDestination> mapFunc,
        CancellationToken cancellationToken = default
    );
}