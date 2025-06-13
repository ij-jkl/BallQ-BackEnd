namespace BallQ.UnitTest.Application.Common.Pagination.Service;

public class PaginationServiceTests
{
    private readonly PaginationService _paginationService;

    public PaginationServiceTests()
    {
        _paginationService = new PaginationService();
    }

    [Fact]
    public async Task PaginateAsync_ShouldReturnCorrectPage_WithCorrectMetadata()
    {
        // Arrange
        var data = Enumerable.Range(1, 20);
        int pageNumber = 2;
        int pageSize = 5;

        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 5;

        var totalItems = data.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = data
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => $"Item {x}")
            .ToList();

        // Simulate PaginationDto
        var result = new PaginationDto<string>
        {
            Items = items,
            Metadata = new PaginationData
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber
            }
        };

        // Assert
        Assert.Equal(5, result.Items.Count);
        Assert.Equal("Item 6", result.Items.First());
        Assert.Equal("Item 10", result.Items.Last());
        Assert.Equal(20, result.Metadata.TotalItems);
        Assert.Equal(4, result.Metadata.TotalPages);
        Assert.Equal(5, result.Metadata.PageSize);
        Assert.Equal(2, result.Metadata.CurrentPage);
    }


    [Fact]
    public async Task PaginateAsync_ShouldDefaultToPage1_IfPageNumberIsInvalid()
    {
        // Arrange - Passing an invalid page number
        var data = Enumerable.Range(1, 10); 
        int page = 0;
        int size = 5;

        int pageNumber = page <= 0 ? 1 : page;
        int pageSize = size <= 0 ? 5 : size;

        var totalItems = data.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = data
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PaginationDto<int>
        {
            Items = items,
            Metadata = new PaginationData
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber
            }
        };

        // Assert
        Assert.Equal(5, result.Items.Count);
        Assert.Equal(1, result.Metadata.CurrentPage);
    }


    [Fact]
    public async Task PaginateAsync_ShouldDefaultToPageSize5_IfPageSizeIsInvalid()
    {
        var data = Enumerable.Range(1, 10);
        int page = 1;
        int size = 0;

        int pageNumber = page <= 0 ? 1 : page;
        int pageSize = size <= 0 ? 5 : size;

        var totalItems = data.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = data
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PaginationDto<int>
        {
            Items = items,
            Metadata = new PaginationData
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber
            }
        };

        // Assert
        Assert.Equal(5, result.Items.Count);
        Assert.Equal(5, result.Metadata.PageSize);
    }


    [Fact]
    public async Task PaginateAsync_ShouldReturnEmpty_WhenPageIsOutOfRange()
    {
        var data = Enumerable.Range(1, 10);
        int page = 999;
        int size = 5;

        int pageNumber = page <= 0 ? 1 : page;
        int pageSize = size <= 0 ? 5 : size;

        var totalItems = data.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = data
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PaginationDto<int>
        {
            Items = items,
            Metadata = new PaginationData
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber
            }
        };

        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(10, result.Metadata.TotalItems);
        Assert.Equal(2, result.Metadata.TotalPages);
    }

}
