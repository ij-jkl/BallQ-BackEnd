namespace Application.Strikers.Commands.UpdateStriker;

public class UpdateStrikerCommand : IRequest<ResponseObjectJsonDto>
{
    public UpdateStrikerDto Striker { get; set; }
}

public class UpdateStrikerCommandHandler : IRequestHandler<UpdateStrikerCommand, ResponseObjectJsonDto>
{
    private readonly IStrikerRepository _repository;
    private readonly IMapper _mapper;

    public UpdateStrikerCommandHandler(IStrikerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseObjectJsonDto> Handle(UpdateStrikerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var striker = await _repository.GetById(command.Striker.Id);

            if (striker == null)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodeHttp.NOTFOUND,
                    Message = "The striker you are trying to update does not exist.",
                    Response = null
                };
            }

            var dtoProperties = command.Striker.GetType().GetProperties();
            var entityProperties = striker.GetType().GetProperties();

            foreach (var prop in dtoProperties)
            {
                if (prop.Name == "Id") continue; 

                var newValue = prop.GetValue(command.Striker);
                if (newValue != null)
                {
                    var entityProp = entityProperties.FirstOrDefault(p => p.Name == prop.Name);
                    if (entityProp != null && entityProp.CanWrite)
                    {
                        entityProp.SetValue(striker, newValue);
                    }
                }
            }
            
            var updatedStriker = await _repository.Update(striker);
            var mappedStriker = _mapper.Map<UpdateStrikerDto>(updatedStriker);

            return new ResponseObjectJsonDto()
            {
                Code = (int)CodeHttp.OK,
                Message = "The striker was updated successfully.",
                Response = mappedStriker
            };
        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto()
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = "An exception ocurred when trying to update the striker : " + ex.Message,
                Response = null
            };
        }
    }
}