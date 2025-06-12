namespace Application.Strikers.Commands.CreateStriker;

public class CreateStrikerCommand : IRequest<ResponseObjectJsonDto>
{
    public CreateStrikerDto Striker { get; set; }
}

public class CreateStrikerCommandHandler : IRequestHandler<CreateStrikerCommand, ResponseObjectJsonDto>
{
    private readonly IStrikerRepository _repository;
    private readonly IMapper _mapper;

    public CreateStrikerCommandHandler(IStrikerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<ResponseObjectJsonDto> Handle(CreateStrikerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var newStriker = _mapper.Map<StrikerEntity>(command.Striker);
            
            var createdStriker = await _repository.Create(newStriker);
            
            var mappedStriker = _mapper.Map<CreateStrikerDto>(createdStriker);
            
            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.CREATED,
                Message = "Striker was created successfully.",
                Response = mappedStriker
            };
        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = ex.Message,
                Response = null
            };
        }
    }
}