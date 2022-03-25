namespace MakeYourBusinessGreen.Domain.Entities;
public class Office
{
    public OfficeId Id { get; private set; }
    public OfficeName Name { get; set; }

    public Office(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    private Office()
    {

    }
}
