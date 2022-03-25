using MakeYourBusinessGreen.Domain.Enums;

namespace MakeYourBusinessGreen.Application.Models;
public class StatusChangedEventReadModel
{
    public Guid Id { get; set; }
    public Status From { get; set; }
    public Status To { get; set; }
    public DateTime DateTime { get; set; }
    public string Details { get; set; }
    public string ModeratorId { get; set; }
    public virtual SuggestionReadModel Suggestion { get; set; }
}
