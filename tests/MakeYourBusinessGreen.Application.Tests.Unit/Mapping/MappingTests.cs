using MakeYourBusinessGreen.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Mapping;
public class MappingTests
{

    [Fact]
    public void OfficeReadModelMapper_ShouldMapToOfficeResponse()
    {
        // Arrange
        var office = new OfficeReadModel { Id = Guid.NewGuid(), Name = "name" };

        // Act
        var result = office.ToOfficeResponse();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OfficeResponse>();
        result.Name.Should().Be(office.Name);
    }


    [Fact]
    public void SuggestionReadModelMapper_ShouldMapToSuggestionResponse()
    {
        // Arrange
        var office = new OfficeReadModel { Id = Guid.NewGuid(), Name = "name" };
        var suggestion = new SuggestionReadModel
        {
            Id = Guid.NewGuid(),
            Title = "Title",
            Body = "Body",
            Created = DateTime.UtcNow,
            Office = office,
            Status = Status.Archived,
            UserId = Guid.NewGuid().ToString(),
            StatusChangedEvents = new List<StatusChangedEventReadModel>
            {
                new StatusChangedEventReadModel
                {

                    DateTime = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    Details = "Details",
                    From = Status.Pending,
                    To = Status.Archived,
                    ModeratorId = Guid.NewGuid().ToString(),
                    Suggestion = null
                }
            }
        };
        suggestion.Office = office;

        // Act
        var result = suggestion.ToSuggestionResponse();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<SuggestionResponse>();
        result.Title.Should().Be(suggestion.Title);
        result.Body.Should().Be(suggestion.Body);
        result.Status.Should().Be(suggestion.Status.ToString());
        result.UserId.Should().Be(suggestion.UserId);
        result.Office.Should().Be(suggestion.Office.ToOfficeResponse());
        var statusEventResponse = result.StatusChangedEvents.ToList().First();
        var statusEvent = suggestion.StatusChangedEvents.First();
        statusEventResponse.DateTime.Should().Be(statusEvent.DateTime);
        statusEventResponse.To.Should().Be(statusEvent.To.ToString());
        statusEventResponse.From.Should().Be(statusEvent.From.ToString());
        statusEvent.Details.Should().Be(statusEvent.Details);
    }
}
