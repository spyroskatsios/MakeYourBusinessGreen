namespace MakeYourBusinessGreen.Application.Commands.OfficeCommands.Handlers;
public class DeleteOfficeHandler : IRequestHandler<DeleteOfficeCommand, bool>
{
    private readonly IRepositoryManager _repository;

    public DeleteOfficeHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }


    public async Task<bool> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = await _repository.Office.GetAsync(request.Id);

        if (office is null)
        {
            return false;
        }

        await _repository.Office.DeleteAsync(office);
        return true;
    }
}
