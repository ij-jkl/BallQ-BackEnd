namespace Application.RatePlayers.Queries;

public class GetAllRatingsQuery : IRequest<ResponseObjectJsonDto>
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}

public class GetAllRatingsQueryHandler : IRequestHandler<GetAllRatingsQuery, ResponseObjectJsonDto>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly IPaginationService _paginationService;
    private readonly IMapper _mapper;
    
    public GetAllRatingsQueryHandler(IPlayerRatingRepository playerRatingRepository, IPaginationService paginationService, IMapper mapper)
    {
        _playerRatingRepository = playerRatingRepository;
        _paginationService = paginationService;
        _mapper = mapper;
    }

    public async Task<ResponseObjectJsonDto> Handle(GetAllRatingsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var page = query.PageNumber.GetValueOrDefault(1);
            var size = query.PageSize.GetValueOrDefault(5);

            var calificationQuery = _playerRatingRepository.AsQueryable();
            
            var result = await _paginationService.PaginateAsync(
                calificationQuery,
                page,
                size,
                _mapper.Map<GetRatingDto>, 
                cancellationToken
            );
            
            return new ResponseObjectJsonDto
            {
                Code = 200,
                Message = "The list of ratings was successfully retrieved.",
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