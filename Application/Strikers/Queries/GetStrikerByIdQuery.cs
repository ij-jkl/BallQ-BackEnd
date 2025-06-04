namespace Application.Strikers.Queries;

public class GetStrikerByIdQuery : IRequest<ResponseObjectJsonDto>
{
    public int Id { get; set; }
    
    public GetStrikerByIdQuery() { }
}

public class GetStrikerByIdQueryHandler : IRequestHandler<GetStrikerByIdQuery, ResponseObjectJsonDto>
{
    private readonly IStrikerRepository _repository;
    private readonly IMapper _mapper;

    public GetStrikerByIdQueryHandler(IStrikerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseObjectJsonDto> Handle(GetStrikerByIdQuery command, CancellationToken cancellationToken)
    {
        try
        {
            var strikerById = await _repository.GetById(command.Id);
            
            if (strikerById == null)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodeHttp.NOTFOUND,
                    Message = $"There is no striker with ID : ({command.Id})"
                };
            }

            var mappedStriker = _mapper.Map<GetStrikerDto>(strikerById);
            
            return new ResponseObjectJsonDto()
            {
                Code = (int)CodeHttp.OK,
                Message = $"The striker with ID ({command.Id}) is : ",
                Response = mappedStriker
            };
            
        }catch (Exception ex)
        {
            return new ResponseObjectJsonDto()
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = ex.Message,
                Response = null
            };
        }
    }
}
