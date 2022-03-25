using MakeYourBusinessGreen.Domain.Enums;

namespace MakeYourBusinessGreen.Application.Models;
public class SuggestionReadModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public virtual OfficeReadModel Office { get; set; }
    public Status Status { get; set; }
    public string UserId { get; set; }
    public DateTime Created { get; set; }
    public virtual ICollection<StatusChangedEventReadModel> StatusChangedEvents { get; set; } = new HashSet<StatusChangedEventReadModel>();
}
