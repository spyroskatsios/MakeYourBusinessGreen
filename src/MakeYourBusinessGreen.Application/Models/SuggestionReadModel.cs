using MakeYourBusinessGreen.Domain.Enums;

namespace MakeYourBusinessGreen.Application.Models;
public class SuggestionReadModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public virtual OfficeReadModel Office { get; set; } = default!;
    public Status Status { get; set; }
    public string UserId { get; set; } = default!;
    public DateTime Created { get; set; }
    public virtual ICollection<StatusChangedEventReadModel> StatusChangedEvents { get; set; } = new HashSet<StatusChangedEventReadModel>();
}
