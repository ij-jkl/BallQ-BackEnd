namespace Presentation.Controllers;

[ApiController]
[Route("api/rating")]
public class RatingController : ApiControllerBase
{
    [HttpPost("rate-all-strikers")]
    public async Task<ActionResult<ResponseObjectJsonDto>> RateAllStrikers()
    {
        var result = await Mediator.Send(new RateAllStrikersCommand());
        return StatusCode(result.Code, result);
    }
    
    [HttpGet("get_all_rated_strikers")]
    public async Task<ActionResult<ResponseObjectJsonDto>> GetAllRatings([FromQuery] GetAllRatingsQuery getAllRatingsQuery)
    {
        var listOfRatedStrikers = await Mediator.Send(getAllRatingsQuery);

        return StatusCode(listOfRatedStrikers.Code, listOfRatedStrikers);
    }
    
    [HttpGet("get_rated_strikers_by_{id}")]
    public async Task<ActionResult<ResponseObjectJsonDto>> GetRatingById([FromRoute] int id)
    {
        var query = new GetRatingByIdQuery { Id = id };
        
        var ratedStrikerById = await Mediator.Send(query);
        
        return StatusCode(ratedStrikerById.Code, ratedStrikerById);
    }
    
    [HttpGet("get_top_rated_strikers")]
    public async Task<ActionResult<ResponseObjectJsonDto>> GetTopRatedStrikers([FromQuery] GetTopRatingsQuery getTopRatingsQuery)
    {
        var topRatedStrikers = await Mediator.Send(getTopRatingsQuery);
        return StatusCode(topRatedStrikers.Code, topRatedStrikers);
    }
}