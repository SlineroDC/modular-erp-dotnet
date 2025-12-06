using ERP.Admin.Pages.Products;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;
using Xunit;

namespace ERP.Core.Tests.Admin;

public class CreateProductTests
{
    [Fact]
    public async Task OnPostAsync_WhenModelStateIsValid_ShouldSaveAndRedirect()
    {
        // 1. ARRANGE 
        //We create a fake repository
        var mockRepo = Substitute.For<IProductRepository>();

        //We create the page by injecting the fake one
        var pageModel = new Create(mockRepo);

        //We simulate valid data in the form
        pageModel.Product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Stock = 5,
        };

        // 2. ACT 
        var result = await pageModel.OnPostAsync();

        // 3. ASSERT 
        // A. We verify that the result is a redirect to "./Index" 
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("./Index", redirectResult.PageName);

        // B. We verified that the AddAsync method was called 1 time
        await mockRepo.Received(1).AddAsync(Arg.Any<Product>());
    }

    [Fact]
    public async Task OnPostAsync_WhenModelStateIsInvalid_ShouldReturnPageAndNotSave()
    {
        // 1. ARRANGE
        var mockRepo = Substitute.For<IProductRepository>();
        var pageModel = new Create(mockRepo);

        //We simulate a validation error (e.g. empty name)
        pageModel.ModelState.AddModelError("Product.Name", "Required");

        // 2. ACT
        var result = await pageModel.OnPostAsync();

        // 3. ASSERT
        // A. You must return the same page (PageResult) to display errors
        Assert.IsType<PageResult>(result);

        // B. The AddAsync method should NEVER have been called
        await mockRepo.DidNotReceive().AddAsync(Arg.Any<Product>());
    }
}