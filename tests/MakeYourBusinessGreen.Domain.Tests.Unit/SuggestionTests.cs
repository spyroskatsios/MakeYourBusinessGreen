using FluentAssertions;
using MakeYourBusinessGreen.Domain.Entities;
using MakeYourBusinessGreen.Domain.Enums;
using MakeYourBusinessGreen.Domain.Exceptions;
using MakeYourBusinessGreen.Domain.Exceptions.Suggestion;
using System;
using System.Linq;
using Xunit;

namespace MakeYourBusinessGreen.Domain.Tests.Unit;
public class SuggestionTests
{
    private readonly Suggestion _suggestion;
    private readonly Office _office;

    public SuggestionTests()
    {
        _office = new Office(Guid.NewGuid(), "111");
        _suggestion = new Suggestion(Guid.NewGuid(), "aaa", "aaa", _office, Status.Pending, Guid.NewGuid().ToString());
    }


    [Fact]
    public void CreateSuggestion_ShouldThrowEmptyIdException_WhenIdIsEmpty()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(Guid.Empty, _suggestion.Title, _suggestion.Body, _office, _suggestion.Status, _suggestion.UserId);

        // Assert
        result.Should().Throw<EmptySuggestionIdException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowEmptyTitleException_WhenTitleIsNull()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, null, _suggestion.Body, _office, _suggestion.Status, _suggestion.UserId);

        // Assert
        result.Should().Throw<EmptySuggestionTitleException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowEmptyTitleException_WhenTitleIsEmpty()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, string.Empty, _suggestion.Body, _office, _suggestion.Status, _suggestion.UserId);

        // Assert
        result.Should().Throw<EmptySuggestionTitleException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowEmptyBodyException_WhenBodyIsNull()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, _suggestion.Title, null, _office, _suggestion.Status, _suggestion.UserId);

        // Assert
        result.Should().Throw<EmptySuggestionBodyException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowEmptyBodyException_WhenBodyIsEmpty()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, _suggestion.Title, string.Empty, _office, _suggestion.Status, _suggestion.UserId);

        // Assert
        result.Should().Throw<EmptySuggestionBodyException>();
    }


    [Fact]
    public void CreateSuggestion_ShouldThrowNullOfficeException_WhenOfficeIsNull()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, _suggestion.Title, _suggestion.Body, null, _suggestion.Status, _suggestion.UserId);

        // Assert
        result.Should().Throw<NullOfficeException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowEmptyUserIdException_WhenUserIdIsEmptyGuid()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, _suggestion.Title, _suggestion.Body, _office, _suggestion.Status, Guid.Empty.ToString());

        // Assert
        result.Should().Throw<EmptyUserIdException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowEmptyUserIdException_WhenUserIdIsEmpty()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, _suggestion.Title, _suggestion.Body, _office, _suggestion.Status, string.Empty);

        // Assert
        result.Should().Throw<EmptyUserIdException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowEmptyUserIdException_WhenUserIdIsNull()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, _suggestion.Title, _suggestion.Body, _office, _suggestion.Status, null);

        // Assert
        result.Should().Throw<EmptyUserIdException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowTooLongTitleException_WhenTitleIsTooLong()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, new string('a', 51), _suggestion.Body, _office, _suggestion.Status, _suggestion.UserId);

        // Assert
        result.Should().Throw<TooLongSuggestionTitleException>();
    }

    [Fact]
    public void CreateSuggestion_ShouldThrowTooLongBodyException_WhenBodyIsTooLong()
    {
        // Arrange

        // Act
        Action result = () => new Suggestion(_suggestion.Id, _suggestion.Title, new string('a', 201), _office, _suggestion.Status, _suggestion.UserId);

        // Assert
        result.Should().Throw<TooLongSuggestionBodyException>();
    }

    [Fact]
    public void AddStatusChangedEvent_ShouldThrowEventException_WhenDetailsNull()
    {
        // Arrange

        // Act
        Action result = () => _suggestion.ChangeStatus(Status.Pending, null, Guid.NewGuid().ToString());

        // Assert
        result.Should().Throw<StatusChangedEventException>();
    }

    [Fact]
    public void AddStatusChangedEvent_ShouldThrowEventException_WhenDetailsEmpty()
    {
        // Arrange

        // Act
        Action result = () => _suggestion.ChangeStatus(Status.Pending, string.Empty, Guid.NewGuid().ToString());

        // Assert
        result.Should().Throw<StatusChangedEventException>();
    }

    [Fact]
    public void AddStatusChangedEvent_SHouldThrowEventException_WhenDetailsTooLong()
    {
        // Arrange

        // Act
        Action result = () => _suggestion.ChangeStatus(Status.Pending, new string('a', 501), Guid.NewGuid().ToString());

        // Assert
        result.Should().Throw<StatusChangedEventException>();
    }

    [Fact]
    public void AddStatusChangedEvent_ShouldAddUpdateStatusEvent()
    {
        // Arrange

        // Act
        _suggestion.ChangeStatus(Status.Completed, "Details", Guid.NewGuid().ToString());

        // Assert
        var events = _suggestion.GetStatusChangedEvents().ToList();
        events.Count.Should().Be(1);
        events.First().DateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        _suggestion.Status.Should().Be(Status.Completed);
    }
}
