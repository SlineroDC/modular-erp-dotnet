using ERP.Admin.Pages;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;
using Xunit;

namespace ERP.Core.Tests.Admin;

public class EditCustomerTests
{
    [Fact]
    public async Task OnGetAsync_WhenIdExists_ShouldPopulateCustomer()
    {
        // 1. ARRANGE
        var mockRepo = Substitute.For<ICustomerRepository>();
        var expectedCustomer = new Customer { Id = 1, Name = "Juan" };

        // Le enseñamos al mock: "Cuando te pidan el ID 1, devuelve a Juan"
        mockRepo.GetByIdAsync(1).Returns(expectedCustomer);

        var pageModel = new EditCustomerModel(mockRepo);

        // 2. ACT
        var result = await pageModel.OnGetAsync(1);

        // 3. ASSERT
        Assert.IsType<PageResult>(result); // Debe cargar la página
        Assert.NotNull(pageModel.Customer); // La propiedad debe tener datos
        Assert.Equal("Juan", pageModel.Customer.Name); // Debe ser Juan
    }

    [Fact]
    public async Task OnGetAsync_WhenIdDoesNotExist_ShouldReturnNotFound()
    {
        // 1. ARRANGE
        var mockRepo = Substitute.For<ICustomerRepository>();

        // Le enseñamos: "Cuando te pidan el ID 99, devuelve null (no existe)"
        mockRepo.GetByIdAsync(99).Returns((Customer?)null);

        var pageModel = new EditCustomerModel(mockRepo);

        // 2. ACT
        var result = await pageModel.OnGetAsync(99);

        // 3. ASSERT
        // Debe retornar NotFound (404), no PageResult
        Assert.IsType<NotFoundResult>(result);
    }
}
