namespace MakeYourBusinessGreen.Api.Controllers;

[ApiController]
[Authorize]
public class SuggestionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuggestionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Routes.Suggestion.Get)]
    public async Task<ActionResult> Get([FromRoute] GetSuggestionByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet(Routes.Suggestion.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSuggestionsQuery query, CancellationToken cancellationToken)
    {
        var suggestions = await _mediator.Send(query, cancellationToken);
        return Ok(suggestions);
    }

    [HttpPost(Routes.Suggestion.Create)]
    public async Task<IActionResult> Create(CreateSuggestionCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = id }, null);
    }

    [HttpPut(Routes.Suggestion.UpdateStatus)]
    [Authorize(Roles = Roles.Moderator)]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateSuggestionStatusCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    [HttpDelete(Routes.Suggestion.Delete)]
    [Authorize(Roles = Roles.Moderator)]
    public async Task<IActionResult> Delete([FromRoute] DeleteSuggestionCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    [HttpGet(Routes.Suggestion.GetForCurrentUser)]
    public async Task<IActionResult> GetByStatus([FromQuery] GetSuggestionsByStatusQuery query, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }


    [HttpGet(Routes.Suggestion.SearchByTitle)]
    public async Task<IActionResult> SearchByTitle([FromRoute] SearchSuggestionByTitleQuery query, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

}
