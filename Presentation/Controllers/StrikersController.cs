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

    [HttpGet("get_all_strikers")]
    public async Task<ActionResult<ResponseObjectJsonDto>> GetAllStrikers([FromQuery] GetAllStrikersQuery getAllStrikersQuery)
    {
        var listOfStrikers = await Mediator.Send(getAllStrikersQuery);

        return StatusCode(listOfStrikers.Code, listOfStrikers);
    }
    
    [HttpGet("search_by_name")]
    public async Task<IActionResult> SearchStrikersByName([FromQuery] string strikerName)
    {
        var strikersByName = await Mediator.Send(new GetStrikerByNameQuery(strikerName));
        return StatusCode(strikersByName.Code, strikersByName);
    }

}