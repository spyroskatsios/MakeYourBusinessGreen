namespace MakeYourBusinessGreen.Application.Commands.OfficeCommands.Handlers;
public class UpdateOfficeHandler : IRequestHandler<UpdateOfficeCommand, bool>
{
    private readonly IRepositoryManager _repository;

    public UpdateOfficeHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = await _repository.Office.GetAsync(request.Id);

        if (office is null)
        {
            return false;
        }

        office.Name = request.Name;

        await _repository.Office.UpdateAsync(office);
        return true;
    }
}
