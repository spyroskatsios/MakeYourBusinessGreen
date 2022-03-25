namespace MakeYourBusinessGreen.Domain.Repositories;
public interface IRepositoryManager
{
    IOfficeRepository Office { get; }
    ISuggestionRepository Suggestion { get; }
}
