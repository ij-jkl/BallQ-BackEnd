namespace Application.Strikers.Queries;

public class GetStrikerByNameQuery : IRequest<ResponseObjectJsonDto>
{
    public string Query { get; set; }

    public GetStrikerByNameQuery(string query)
    {
        Query = query;
    }
}

public class GetStrikerByNameQueryHandler : IRequestHandler<GetStrikerByNameQuery, ResponseObjectJsonDto>
{
    private readonly IStrikerRepository _repository;
    private readonly IMapper _mapper;

    public GetStrikerByNameQueryHandler(IStrikerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseObjectJsonDto> Handle(GetStrikerByNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Query))
            {
                return new ResponseObjectJsonDto
                {
                    Code = (int)CodeHttp.BADREQUEST,
                    Message = "Search query cannot be empty.",
                    Response = null
                };
            }

            var strikerQuery = _repository.AsQueryable();

            var filteredStrikers = await strikerQuery
                .Where(s => EF.Functions.Like(s.Name.ToLower(), $"%{request.Query.ToLower()}%"))
                .ToListAsync(cancellationToken);

            if (!filteredStrikers.Any())
            {
                return new ResponseObjectJsonDto
                {
                    Code = (int)CodeHttp.NOTFOUND,
                    Message = $"No strikers found matching the name '{request.Query}'.",
                    Response = null
                };
            }

            var result = _mapper.Map<List<GetStrikerDto>>(filteredStrikers);

            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.OK,
                Message = $"Found {result.Count} striker(s) matching '{request.Query}'.",
                Response = result
            };
        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = $"Error occurred while searching: {ex.Message}",
                Response = null
            };
        }
    }
}