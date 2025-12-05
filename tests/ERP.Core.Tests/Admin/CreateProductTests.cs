using ERP.Admin.Pages.Products;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;
using Xunit;

public class CreateProductTests
{
    [Fact]
    public async Task OnPostAsync_WhenModelStateIsValid_ShouldSaveAndRedirect()
    {
        // 1. ARRANGE (Preparar)
        // Creamos un repositorio falso
        var mockRepo = Substitute.For<IProductRepository>();

        // Creamos la página inyectándole el falso
        var pageModel = new CreateProductModel(mockRepo);

        // Simulamos datos válidos en el formulario
        pageModel.Product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Stock = 5,
        };

        // 2. ACT (Actuar)
        var result = await pageModel.OnPostAsync();

        // 3. ASSERT (Verificar)
        // A. Verificamos que el resultado sea una redirección a "./Products"
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("./Products", redirectResult.PageName);

        // B. ¡Lo más importante! Verificamos que el método AddAsync FUE LLAMADO 1 vez
        await mockRepo.Received(1).AddAsync(Arg.Any<Product>());
    }

    [Fact]
    public async Task OnPostAsync_WhenModelStateIsInvalid_ShouldReturnPageAndNotSave()
    {
        // 1. ARRANGE
        var mockRepo = Substitute.For<IProductRepository>();
        var pageModel = new CreateProductModel(mockRepo);

        // Simulamos un error de validación (ej. Nombre vacío)
        pageModel.ModelState.AddModelError("Product.Name", "Required");

        // 2. ACT
        var result = await pageModel.OnPostAsync();

        // 3. ASSERT
        // A. Debe retornar la misma página (PageResult) para mostrar errores
        Assert.IsType<PageResult>(result);

        // B. El método AddAsync NUNCA debió llamarse
        await mockRepo.DidNotReceive().AddAsync(Arg.Any<Product>());
    }
}
