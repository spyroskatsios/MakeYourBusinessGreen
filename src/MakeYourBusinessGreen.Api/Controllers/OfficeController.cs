namespace MakeYourBusinessGreen.Api.Controllers;

//[Authorize]
[ApiController]
public class OfficeController : ControllerBase
{
    private readonly IMediator _mediator;

    public OfficeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Routes.Office.Get)]
    public async Task<IActionResult> Get([FromRoute] GetOfficeByIdQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        var response = await _mediator.Send(query, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }


    [HttpGet(Routes.Office.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOfficesQuery query, CancellationToken cancellation)
    {
        var response = await _mediator.Send(query, cancellation);
        return Ok(response);
    }

    [HttpPost(Routes.Office.Create)]
    [Authorize(Roles = Roles.Moderator)]
    public async Task<IActionResult> Create(CreateOfficeCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { Id = id }, null);
    }

    [HttpPut(Routes.Office.Update)]
    [Authorize(Roles = Roles.Moderator)]
    public async Task<IActionResult> Update([FromBody] UpdateOfficeCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    [HttpDelete(Routes.Office.Delete)]
    [Authorize(Roles = Roles.Moderator)]
    public async Task<IActionResult> Delete([FromRoute] DeleteOfficeCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }


    [HttpGet(Routes.Office.SearchByName)]
    public async Task<IActionResult> SearchByName([FromQuery] SearchOfficeByNameQuery query, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
}
