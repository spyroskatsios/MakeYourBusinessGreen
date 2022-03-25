using System;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Commands.SuggestionCommands;
public class CreateSuggestionCommandTests
{
    private readonly CreateSuggestionHandler _sut;
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly CancellationToken _cancellationToken = default;
    private readonly IRepositoryManager _repository = Substitute.For<IRepositoryManager>();

    public CreateSuggestionCommandTests()
    {
        _sut = new(_repository, _currentUserService);
    }

    [Fact]
    public async Task Create_ShouldReturnEmptyGuid_WhenOfficeNotFound()
    {
        // Arrange
        var command = new CreateSuggestionCommand("Title", "Body", Guid.NewGuid());
        _currentUserService.Id.Returns(Guid.NewGuid().ToString());
        _repository.Office.GetAsync(command.OfficeId).ReturnsNull();

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        result.Should().Be(Guid.Empty);
        await _repository.Suggestion.DidNotReceive().AddAsync(Arg.Any<Suggestion>());
    }

    [Fact]
    public async Task Create_ShouldSaveSuggestion_WhenInvoked()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var office = new Office(Guid.NewGuid(), "1");
        var command = new CreateSuggestionCommand("Title", "Body", office.Id);
        _currentUserService.Id.Returns(userId);
        _repository.Office.GetAsync(command.OfficeId).Returns(office);

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        result.Should().NotBe(Guid.Empty);
        await _repository.Suggestion.Received(1).AddAsync(Arg.Is<Suggestion>(
            x => x.Office == office && x.Status == Status.Pending && x.Title == command.Title && x.Body == command.Body));
    }

}
