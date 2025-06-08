namespace Application.RatePlayers.Commands.RateAllStrikers;

public class RateAllStrikersCommand : IRequest<ResponseObjectJsonDto>
{
    
}

public class RateAllStrikersCommandHandler : IRequestHandler<RateAllStrikersCommand, ResponseObjectJsonDto>
{
    private readonly IStrikerRepository _strikerRepository;
    private readonly IPlayerRatingRepository _ratingRepository;
    private readonly IPlayerRatingService<StrikerEntity, RatingEntity> _ratingService;

    public RateAllStrikersCommandHandler(
        IStrikerRepository strikerRepository,
        IPlayerRatingRepository ratingRepository,
        IPlayerRatingService<StrikerEntity, RatingEntity> ratingService)
    {
        _strikerRepository = strikerRepository;
        _ratingRepository = ratingRepository;
        _ratingService = ratingService;
    }

    public async Task<ResponseObjectJsonDto> Handle(RateAllStrikersCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var allStrikers = await _strikerRepository.GetAllStrikers();
            var ratingsToSave = new List<RatingEntity>();

            foreach (var striker in allStrikers)
            {
                var rating = await _ratingService.GenerateStrikerRating(striker, allStrikers);
                ratingsToSave.Add(rating);
            }

            await _ratingRepository.SaveAllStrikers(ratingsToSave);

            return new ResponseObjectJsonDto
            {
                Code = 200,
                Message = "Ratings calculated and upserted successfully.",
                Response = null
            };
        }
        catch (Exception ex)
        {
            return new ResponseObjectJsonDto
            {
                Code = 500,
                Message = ex.Message,
                Response = null
            };
        }
    }
}
