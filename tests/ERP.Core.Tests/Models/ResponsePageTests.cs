using ERP.Core.Models;
using Xunit;

namespace ERP.Core.Tests.Models;

public class ResponsePageTests
{
    [Theory]
    // Case 1: 10 items, pages of 10 = 1 page exactly
    [InlineData(10, 10, 1)]
    // Case 2: 11 items, pages de 10 = 2 page 
    [InlineData(11, 10, 2)]
    // Case 3: 0 items = 0 pages
    [InlineData(0, 10, 0)]
    // Case 4: 100 items, pages of 10 = 10 pages
    [InlineData(100, 10, 10)]
    // Case 5: 5 items, pages of 2 = 3 pages (2 + 2 + 1)
    [InlineData(5, 2, 3)]
    public void TotalPages_Calculation_ShouldBeCorrect(int totalCount, int pageSize, int expectedTotalPages)
    {
        // Arrange: We create the simulated response
        var page = new ResponsePage<object>
        {
            TotalCount = totalCount,
            PageSize = pageSize
        };

        // Act: We get the calculation
        var result = page.TotalPages;

        // Assert: We verify that the mathematics is correct
        Assert.Equal(expectedTotalPages, result);
    }
    
    [Fact]
    public void Items_ShouldInitializeEmpty_NotNull()
    {
        // Arrange
        var page = new ResponsePage<string>();

        // Assert
        Assert.NotNull(page.Items);
        Assert.Empty(page.Items);
    }
}