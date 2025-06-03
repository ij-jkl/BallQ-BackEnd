namespace Presentation.Controllers;

[Route("api/strikers")]
[ApiController]    

public class StrikersController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ResponseObjectJsonDto>> CreateStriker([FromBody] CreateStrikerCommand createStrikerCommand)
    {
        var createStriker = await Mediator.Send(createStrikerCommand);
        
        return StatusCode(createStriker.Code, createStriker);
    }
}