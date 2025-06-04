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
    
    [HttpPut]
    public async Task<ActionResult<ResponseObjectJsonDto>> UpdateStriker([FromBody] UpdateStrikerCommand updateStrikerCommand)
    {
        var updateStriker = await Mediator.Send(updateStrikerCommand);
        
        return StatusCode(updateStriker.Code, updateStriker);
    }
}