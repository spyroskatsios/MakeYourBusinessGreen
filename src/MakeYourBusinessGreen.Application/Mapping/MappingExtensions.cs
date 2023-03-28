using MakeYourBusinessGreen.Application.Models;

namespace MakeYourBusinessGreen.Application.Mapping;
public static class MappingExtensions
{
    public static OfficeResponse? ToOfficeResponse(this OfficeReadModel? office)
        => office is null ? null : new OfficeResponse(office.Id, office.Name);

    public static List<OfficeResponse> ToOfficesResponse(this IEnumerable<OfficeReadModel> offices)
        => offices.Select(office => office.ToOfficeResponse()!).ToList();

    public static SuggestionResponse? ToSuggestionResponse(this SuggestionReadModel? suggestion)
        => suggestion is null ? null : new SuggestionResponse(suggestion.Id, suggestion.Title, suggestion.Body, suggestion.Office.ToOfficeResponse()!,
            suggestion.Status.ToString(), suggestion.UserId, suggestion.StatusChangedEvents.ToStatusChangedEventResponse());

    public static List<SuggestionResponse> ToSuggestionsResponse(this IEnumerable<SuggestionReadModel> suggestions) 
        => suggestions.Select(suggestion => suggestion.ToSuggestionResponse()!).ToList();

    private static List<StatusChangedEventResponse> ToStatusChangedEventResponse(this IEnumerable<StatusChangedEventReadModel> events) 
        => events.Select(eventsStatus => new StatusChangedEventResponse(eventsStatus.From.ToString(), eventsStatus.To.ToString(), eventsStatus.DateTime, eventsStatus.Details)).ToList();
}
