using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.RequestFeatures;
public class PagedListTests
{
    [Theory]
    [InlineData(10, 1, true, false)]
    [InlineData(10, 10, false, true)]
    [InlineData(10, 5, true, true)]
    public void ToPagedList_ShouldReturnPagedListWithCorrectProperties(int pageSize, int pageNumber, bool hasNext, bool hasPrevious)
    {
        // Arrange
        var testList = new List<string>();

        for (int i = 0; i < 95; i++)
        {
            testList.Add(i.ToString());
        }

        // Act
        var result = PagedList<string>.ToPagedList(testList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(), pageNumber, pageSize, testList.Count);

        // Assert
        result.Count.Should().Be(testList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList().Count);
        result.MetaData.TotalCount.Should().Be(testList.Count);
        result.MetaData.TotalPages.Should().Be((int)Math.Ceiling(testList.Count / (double)pageSize));
        result.MetaData.HasNextPage.Should().Be(hasNext);
        result.MetaData.HasPreviousPage.Should().Be(hasPrevious);
    }


}
