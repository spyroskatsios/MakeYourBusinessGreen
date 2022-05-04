namespace MakeYourBusinessGreen.Domain.Entities;


public class StatusChangedEvent
{
    public Status From { get; set; }
    public Status To { get; set; }
    public DateTime DateTime { get; set; }
    public string Details { get; set; }
    public UserId ModeratorId { get; set; }

    public StatusChangedEvent(Status from, Status to, string details, string moderatorId)
    {
        if (string.IsNullOrWhiteSpace(details))
        {
            throw new StatusChangedEventException("StatusChangedEvent Details can not be empty.");
        }

        if (details.Length > 500)
        {
            throw new StatusChangedEventException($"StatusChangedEvent Details can not be more than {500} characters.");
        }


        From = from;
        To = to;
        DateTime = DateTime.UtcNow;
        ModeratorId = moderatorId;
        Details = details;
    }

    private StatusChangedEvent()
    {

    }
}
