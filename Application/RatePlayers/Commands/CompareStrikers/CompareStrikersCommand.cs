namespace Application.RatePlayers.Commands.CompareStrikers;

public class CompareStrikersCommand : IRequest<ResponseObjectJsonDto>
{
    public List<int> PlayerIds { get; set; } = new();
}

public class CompareStrikersCommandHandler : IRequestHandler<CompareStrikersCommand, ResponseObjectJsonDto>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly IMapper _mapper;

    public CompareStrikersCommandHandler(IPlayerRatingRepository playerRatingRepository, IMapper mapper)
    {
        _playerRatingRepository = playerRatingRepository;
        _mapper = mapper;
    }

    public async Task<ResponseObjectJsonDto> Handle(CompareStrikersCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var finalIds = await PlayerIdResolver.ResolveFinalIdsTask(command.PlayerIds, _playerRatingRepository);

            var ratings = await _playerRatingRepository.GetMultiplePlayerByIds(finalIds);

            var result = ratings.Select(r => new RatingComparisonDto
            {
                PlayerId = r.PlayerId,
                Name = r.Player?.Name ?? "Unknown",
                GoalScore = r.GoalScore,
                PassingScore = r.PassingScore,
                ShootingScore = r.ShootingScore,
                InvolvementScore = r.InvolvementScore,
                FinalScore = r.FinalScore
            }).ToList();

            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.OK,
                Message = $"Ratings between players [{string.Join(", ", finalIds)}] compared successfully.",
                Response = result
            };
        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHttp.INTERNALSERVER,
                Message = "Exception during comparison : " + ex.Message,
                Response = null
            };
        }
    }
}
