namespace Presentation.Controllers;

[Route("api/strikers")]
[ApiController]    

public class StrikersController : ApiControllerBase
{
    [HttpPost("create_striker")]
    public async Task<ActionResult<ResponseObjectJsonDto>> CreateStriker([FromBody] CreateStrikerCommand createStrikerCommand)
    {
        var createStriker = await Mediator.Send(createStrikerCommand);
        
        return StatusCode(createStriker.Code, createStriker);
    }
    
    [HttpPut("update_striker")]
    public async Task<ActionResult<ResponseObjectJsonDto>> UpdateStriker([FromBody] UpdateStrikerCommand updateStrikerCommand)
    {
        var updateStriker = await Mediator.Send(updateStrikerCommand);
        
        return StatusCode(updateStriker.Code, updateStriker);
    }
    
    [HttpGet("get_striker_by_{id}")]
    public async Task<ActionResult<ResponseObjectJsonDto>> GetStrikerById([FromRoute] int id)
    {
        var query = new GetStrikerByIdQuery { Id = id };
        
        var result = await Mediator.Send(query);
        
        return StatusCode(result.Code, result);
    }

}