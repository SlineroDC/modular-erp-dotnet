using ERP.Core.Models;

namespace ERP.Core.Tests.Modals;

public class ResponsePageTest
{
    [Theory]
    // Case 1: 10 items, page de 10 = 1 pages exactly
    [InlineData(10, 10, 1)]
    // Case 2: 11 items, page de 10 = 2 pages 
    [InlineData(11, 10, 2)]
    // Case 3: 0 items = 0 pages
    [InlineData(0, 10, 0)]
    // Case 4: 100 items, page de 10 = 10 pages
    [InlineData(100, 10, 10)]
    // Case 5: 5 items, page de 2 = 3 pages (2 + 2 + 1)
    [InlineData(5, 2, 3)]
    public void TotalPages_Calculation_ShouldBeCorrect(int totalCount, int pageSize, int expectedTotalPages)
    {
        // Arrange: Create simulate response
        var page = new ResponsePage<object>
        {
            TotalCount = totalCount,
            PageSize = pageSize
        };

        // Act: calculate
        var result = page.TotalPages;

        // Assert: Verify calcule
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

