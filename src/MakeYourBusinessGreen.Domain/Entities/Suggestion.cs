namespace MakeYourBusinessGreen.Domain.Entities;
public class Suggestion
{
    public SuggestionId Id { get; private set; }
    public SuggestionTitle Title { get; private set; }
    public SuggestionBody Body { get; private set; }
    public Office Office { get; set; }
    public Status Status { get; private set; }
    public UserId UserId { get; private set; }
    public DateTime Created { get; private set; }
    private ICollection<StatusChangedEvent> _statusChangedEvents { get; set; } = new List<StatusChangedEvent>();

    public Suggestion(Guid id, string title, string body, Office office, Status status, string userId)
    {
        if (office is null)
        {
            throw new NullOfficeException();
        }

        Id = id;
        Title = title;
        Body = body;
        Office = office;
        Status = status;
        UserId = userId;
        Created = DateTime.UtcNow;
    }

    private Suggestion()
    {

    }

    public IEnumerable<StatusChangedEvent> GetStatusChangedEvents()
        => _statusChangedEvents;

    public void ChangeStatus(Status to, string details, string moderatorId)
    {
        _statusChangedEvents.Add(new StatusChangedEvent(Status, to, details, moderatorId));
        Status = to;
    }
}
