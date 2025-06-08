namespace Presentation.Controllers;

[ApiController]
[Route("api/load-data")]
public class DataLoadController : ApiControllerBase
{
    [HttpPost("load-strikers")]
    public async Task<ActionResult<ResponseObjectJsonDto>> LoadStrikers([FromBody] LoadStrikersCommand command)
    {
        var result = await Mediator.Send(new LoadStrikersCommand());
        
        return StatusCode(result.Code, result);
    }
}
