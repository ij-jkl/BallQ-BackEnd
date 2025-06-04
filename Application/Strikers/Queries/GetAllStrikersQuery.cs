namespace Application.Strikers.Queries;

public class GetAllStrikersQuery : IRequest<ResponseObjectJsonDto>
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}

public class GetAllStrikersQueryHandler : IRequestHandler<GetAllStrikersQuery, ResponseObjectJsonDto>
{
    private readonly IStrikerRepository _repository;
    private readonly IPaginationService _paginationService;
    private readonly IMapper _mapper;

    public GetAllStrikersQueryHandler(IStrikerRepository repository, IPaginationService paginationService, IMapper mapper)
    {
        _repository = repository;
        _paginationService = paginationService;
        _mapper = mapper;
    }

    public async Task<ResponseObjectJsonDto> Handle(GetAllStrikersQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var page = query.PageNumber.GetValueOrDefault(1);
            var size = query.PageSize.GetValueOrDefault(5);

            var strikerQuery = _repository.AsQueryable();

            var result = await _paginationService.PaginateAsync(
                strikerQuery,
                page,
                size,
                _mapper.Map<GetStrikerDto>,
                cancellationToken
                );
            
            if (result.Items == null || !result.Items.Any())
            {
                return new ResponseObjectJsonDto
                {
                    Code = (int)CodeHttp.NOTFOUND,
                    Message = "No strikers were found.",
                    Response = null
                };
            }

            return new ResponseObjectJsonDto
            {
                Code = 200,
                Message = "Strikers list retrieved successfully.",
                Response = result
            };
        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = $"Error fetching striker list : {ex.Message}",
                Response = null
            };
        }
    }
}