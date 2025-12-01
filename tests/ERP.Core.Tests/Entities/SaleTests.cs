using ERP.Core.Entities;


namespace ERP.Core.Tests.Entities;

public class SaleTests
{
    [Fact]
    public void NewSale_ShouldBeActiveByDefault()
    {
        // Arrange: Create a new sale instance
        var sale = new Sale();

        // Assert: Verify that IsActive is true by default (Soft Delete requirement)
        Assert.True(sale.IsActive, "New sale should be active upon creation.");
    }

    [Fact]
    public void Sale_ShouldInitializeDetailsList_NotNull()
    {
        // Arrange: Create a new sale instance
        var sale = new Sale();

        // Assert: Ensure SalesDetails collection is initialized to avoid NullReferenceException
        Assert.NotNull(sale.SalesDetails);
        Assert.Empty(sale.SalesDetails);
    }

    [Fact]
    public void CanAddDetailToSale()
    {
        // Arrange: Create sale and a detail item
        var sale = new Sale();
        var detail = new SalesDetail { ProductId = 1, Quantity = 5, UnitPrice = 10.50m };

        // Act: Add the detail to the sale's collection
        sale.SalesDetails.Add(detail);

        // Assert: Verify the detail was added correctly
        Assert.Single(sale.SalesDetails);
        Assert.Equal(1, sale.SalesDetails.First().ProductId);
        Assert.Equal(5, sale.SalesDetails.First().Quantity);
    }
}
