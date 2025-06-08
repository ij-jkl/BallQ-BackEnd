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
}