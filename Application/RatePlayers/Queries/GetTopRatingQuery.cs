namespace Application.RatePlayers.Queries;

public class GetTopRatingsQuery : IRequest<ResponseObjectJsonDto>
{
    public int Limit { get; set; } = 5;
    public bool Ascending { get; set; } = false;
}

public class GetTopRatingsQueryHandler : IRequestHandler<GetTopRatingsQuery, ResponseObjectJsonDto>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly IMapper _mapper;

    public GetTopRatingsQueryHandler(IPlayerRatingRepository playerRatingRepository, IMapper mapper)
    {
        _playerRatingRepository = playerRatingRepository;
        _mapper = mapper;
    }

    public async Task<ResponseObjectJsonDto> Handle(GetTopRatingsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var ratings = await _playerRatingRepository.GetTopRatingsAsync(query.Limit, query.Ascending, cancellationToken);

            var dtoList = _mapper.Map<List<GetRatingDto>>(ratings);

            return new ResponseObjectJsonDto
            {
                Code = 200,
                Message = $"Top {query.Limit} ratings retrieved successfully.",
                Response = new
                {
                    Items = dtoList,
                    Metadata = new
                    {
                        TotalItems = dtoList.Count,
                        Limit = query.Limit,
                        Order = query.Ascending ? "asc" : "desc"
                    }
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = $"Error fetching top ratings: {ex.Message}",
                Response = null
            };
        }
    }
}
