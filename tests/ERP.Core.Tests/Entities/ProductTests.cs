using ERP.Core.Entities;

namespace ERP.Core.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Product_ShouldBeActiveByDefault()
    {
        // Arrange
        var product = new Product();

        // Assert
        Assert.True(product.IsActive, "El producto debe nacer activo.");
    }

    [Fact]
    public void Product_Properties_ShouldStoreValues()
    {
        // Arrange
        var product = new Product
        {
            Name = "Martillo",
            Price = 50.00m,
            Stock = 100
        };

        // Assert
        Assert.Equal("Martillo", product.Name);
        Assert.Equal(50.00m, product.Price);
        Assert.Equal(100, product.Stock);
    }
}