using ERP.Admin.Pages.Customers;
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

        // mock: "When I call GetByIdAsync with ID 1, response Juan"
        mockRepo.GetByIdAsync(1).Returns(expectedCustomer);

        var pageModel = new EditCustomerModel(mockRepo);

        // 2. ACT
        var result = await pageModel.OnGetAsync(1);

        // 3. ASSERT
        Assert.IsType<PageResult>(result); 
        Assert.NotNull(pageModel.Customer); 
        Assert.Equal("Juan", pageModel.Customer.Name); 
    }

    [Fact]
    public async Task OnGetAsync_WhenIdDoesNotExist_ShouldReturnNotFound()
    {
        // 1. ARRANGE
        var mockRepo = Substitute.For<ICustomerRepository>();
        
        mockRepo.GetByIdAsync(99).Returns((Customer?)null);

        var pageModel = new EditCustomerModel(mockRepo);

        // 2. ACT
        var result = await pageModel.OnGetAsync(99);

        // 3. ASSERT
        // NotFound (404)
        Assert.IsType<NotFoundResult>(result);
    }
}
