namespace MakeYourBusinessGreen.Api.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }


    private IActionResult OkOrBadRequest(AuthResponse response)
        => response.Success ? Ok(response) : BadRequest(response);


    [HttpPost(Routes.Auth.SignIn)]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var response = await _mediator.Send(command);
        return OkOrBadRequest(response);
    }

    [HttpPost(Routes.Auth.SignUp)]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        var response = await _mediator.Send(command);
        return OkOrBadRequest(response);
    }


    [HttpPost(Routes.Auth.ForgotPassword)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var response = await _mediator.Send(command);
        return OkOrBadRequest(response);
    }

    [HttpPost(Routes.Auth.ResetPassword)]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand command)
    {
        var response = await _mediator.Send(command);
        return OkOrBadRequest(response);
    }

    [HttpPost(Routes.Auth.ChangePassword)]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var response = await _mediator.Send(command);
        return OkOrBadRequest(response);
    }

    [HttpPost(Routes.Auth.RefreshToken)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var response = await _mediator.Send(command);
        return OkOrBadRequest(response);
    }

    [HttpPost(Routes.Auth.AddUserToRole)]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleCommand command)
    {
        var response = await _mediator.Send(command);
        return OkOrBadRequest(response);
    }

}
