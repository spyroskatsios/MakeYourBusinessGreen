namespace MakeYourBusinessGreen.Application.Responses;
public record SuggestionResponse(Guid Id, string Title, string Body, OfficeResponse Office, string Status, string UserId,
    IEnumerable<StatusChangedEventResponse> StatusChangedEvents)
{

}
