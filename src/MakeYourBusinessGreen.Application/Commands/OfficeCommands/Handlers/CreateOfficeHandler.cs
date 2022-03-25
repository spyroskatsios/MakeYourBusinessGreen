namespace MakeYourBusinessGreen.Application.Commands.OfficeCommands.Handlers;
public class CreateOfficeHandler : IRequestHandler<CreateOfficeCommand, Guid>
{
    private readonly IRepositoryManager _repository;

    public CreateOfficeHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }


    public async Task<Guid> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = new Office(Guid.NewGuid(), request.Name);
        await _repository.Office.AddAsync(office);
        return office.Id;
    }
}
