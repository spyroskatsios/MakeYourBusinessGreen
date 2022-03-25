namespace MakeYourBusinessGreen.Api.Controllers;

[ApiController]
[Authorize(Roles = Roles.Admin)]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Routes.User.Get)]
    public async Task<IActionResult> Get([FromRoute] GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpGet(Routes.User.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpPut(Routes.User.Update)]
    public async Task<IActionResult> Update(UpdateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result == true ? Ok(result) : NotFound();
    }

}
