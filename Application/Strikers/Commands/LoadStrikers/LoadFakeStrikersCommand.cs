namespace Application.Strikers.Commands.LoadStrikers
{
    public class LoadFakeStrikersCommand : IRequest<ResponseObjectJsonDto>
    {
        public int Quantity { get; set; } = 500; 
    }
}

public class LoadFakeStrikersCommandHandler : IRequestHandler<LoadFakeStrikersCommand, ResponseObjectJsonDto>
{
    private readonly IStrikerRepository _repository;
    private readonly IFakeStrikerSeederService _fakerService;

    public LoadFakeStrikersCommandHandler(IStrikerRepository repository, IFakeStrikerSeederService fakerService)
    {
        _repository = repository;
        _fakerService = fakerService;
    }

    public async Task<ResponseObjectJsonDto> Handle(LoadFakeStrikersCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var fakeStrikers = _fakerService.GenerateFakeStrikers(command.Quantity);
            await _repository.AddRangeAsync(fakeStrikers);

            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.OK,
                Message = $"{command.Quantity} fake strikers inserted successfully.",
                Response = null
            };
        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = $"An exception occurred: {ex.Message}",
                Response = null
            };
        }
    }
}