namespace Application.Strikers.Commands.LoadStrikers
{
    public class LoadStrikersCommand : IRequest<ResponseObjectJsonDto>
    {
        
    }
}

public class LoadStrikersCommandHandler : IRequestHandler<LoadStrikersCommand, ResponseObjectJsonDto>
{
    private readonly IDataLoadRepository _repository;

    public LoadStrikersCommandHandler(IDataLoadRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseObjectJsonDto> Handle(LoadStrikersCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _repository.ExecuteStrikerInsertScriptAsync();
            if (!success)
            {
                return new ResponseObjectJsonDto
                {
                    Code = (int)CodeHttp.CONFLICT,
                    Message = "Script execution failed.",
                    Response = null
                };
            }

            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.OK,
                Message = "Strikers inserted successfully.",
                Response = null
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