using MakeYourBusinessGreen.Application.Models;

namespace MakeYourBusinessGreen.Application.Mapping;
public static class MappingExtensions
{
    public static OfficeResponse? ToOfficeResponse(this OfficeReadModel office)
        => office is null ? null : new OfficeResponse(office.Id, office.Name);

    public static List<OfficeResponse> ToOfficesResponse(this IEnumerable<OfficeReadModel> offices)
    {
        var output = new List<OfficeResponse>();

        foreach (var office in offices)
        {
            output.Add(office.ToOfficeResponse());
        }

        return output;
    }

    public static SuggestionResponse? ToSuggestionResponse(this SuggestionReadModel suggestion)
        => suggestion is null ? null : new SuggestionResponse(suggestion.Id, suggestion.Title, suggestion.Body, suggestion.Office.ToOfficeResponse(),
            suggestion.Status.ToString(), suggestion.UserId, suggestion.StatusChangedEvents.ToStatusChangedEventResponse());

    public static List<SuggestionResponse> ToSuggestionsResponse(this IEnumerable<SuggestionReadModel> suggestions)
    {
        var output = new List<SuggestionResponse>();

        foreach (var suggestion in suggestions)
        {
            output.Add(suggestion.ToSuggestionResponse());
        }

        return output;
    }

    private static List<StatusChangedEventResponse> ToStatusChangedEventResponse(this IEnumerable<StatusChangedEventReadModel> events)
    {
        var output = new List<StatusChangedEventResponse>();

        foreach (var eventsStatus in events)
        {
            output.Add(new StatusChangedEventResponse(eventsStatus.From.ToString(), eventsStatus.To.ToString(), eventsStatus.DateTime, eventsStatus.Details));
        }

        return output;
    }

}
