using ERP.Api.Controllers;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace ERP.Api.Tests.Controllers;

public class ProductsControllerTests
{
    [Fact]
    public async Task GetById_WhenProductExists_ReturnsOkAndProduct()
    {
        // 1. Arrange
        var mockRepo = Substitute.For<IProductRepository>();
        var fakeProduct = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 100,
        };

        // Enseñamos al mock a devolver el producto
        mockRepo.GetByIdAsync(1).Returns(fakeProduct);

        var controller = new ProductsController(mockRepo);

        // 2. Act
        var result = await controller.GetById(1);

        // 3. Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // ¿Es 200 OK?
        var returnedProduct = Assert.IsType<Product>(okResult.Value); // ¿Devolvió un producto?
        Assert.Equal(1, returnedProduct.Id);
    }

    [Fact]
    public async Task GetById_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // 1. Arrange
        var mockRepo = Substitute.For<IProductRepository>();
        mockRepo.GetByIdAsync(99).Returns((Product?)null); // Simula que no existe

        var controller = new ProductsController(mockRepo);

        // 2. Act
        var result = await controller.GetById(99);

        // 3. Assert
        Assert.IsType<NotFoundResult>(result); // ¿Es 404 Not Found?
    }
}
