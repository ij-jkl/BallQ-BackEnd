namespace Presentation.Controllers;

[ApiController]
[Route("api/load-data")]
public class DataLoadController : ApiControllerBase
{
    [HttpPost("load-real-strikers")]
    public async Task<ActionResult<ResponseObjectJsonDto>> LoadStrikers([FromBody] LoadStrikersCommand loadStrikersCommand)
    {
        var result = await Mediator.Send(loadStrikersCommand);
        
        return StatusCode(result.Code, result);
    }
    
    [HttpPost("load-fake-strikers")]
    public async Task<ActionResult<ResponseObjectJsonDto>> LoadFakeStrikers([FromBody] LoadFakeStrikersCommand loadFakeStrikersCommand)
    {
        var fakeStrikers = await Mediator.Send(loadFakeStrikersCommand);
        
        return StatusCode(fakeStrikers.Code, fakeStrikers);
    }
}
