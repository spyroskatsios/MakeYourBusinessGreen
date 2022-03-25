namespace MakeYourBusinessGreen.Application.Responses;

public record StatusChangedEventResponse(string From, string To, DateTime DateTime, string Details)
{
}
