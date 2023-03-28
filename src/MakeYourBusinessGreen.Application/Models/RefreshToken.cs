namespace MakeYourBusinessGreen.Application.Models;
public class RefreshToken
{
    public Guid Id { get; set; }

    public string Value { get; set; } = default!;

    public DateTime CreationDateTime { get; set; }

    public DateTime ExpirationDateTime { get; set; }

    public bool Invalidated { get; set; } = false;

    public string UserId { get; set; } = default!;

}
