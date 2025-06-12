namespace Application.RatePlayers.Queries;

public class GetRatingByIdQuery : IRequest<ResponseObjectJsonDto>
{
    public int Id { get; set; }
    
    public GetRatingByIdQuery() { }
}

public class GetRatingByIdQueryHandler : IRequestHandler<GetRatingByIdQuery, ResponseObjectJsonDto>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly IMapper _mapper;

    public GetRatingByIdQueryHandler(IPlayerRatingRepository playerRatingRepository, IMapper mapper)
    {
        _playerRatingRepository = playerRatingRepository;
        _mapper = mapper;
    }

    public async Task<ResponseObjectJsonDto> Handle(GetRatingByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var ratingById = await _playerRatingRepository.GetById(query.Id);
            
            if (ratingById == null)
            {
                return new ResponseObjectJsonDto
                {
                    Code = (int)CodeHttp.NOTFOUND,
                    Message = $"The rating with ID is : ({query.Id})"
                };
            }
            
            var mappedRating = _mapper.Map<GetRatingDto>(ratingById);
            
            return new ResponseObjectJsonDto
            {
                Code = 200,
                Message = $"The rating with the following ID ({query.Id}) is : .",
                Response = mappedRating
            };

        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = $"Error getting rating by ID : {ex.Message}",
                Response = null
            };
        }
    }
}