using ERP.Core.Entities;
using ERP.Core.Interfaces;
using OfficeOpenXml;

namespace ERP.Infrastructure.Services;

public class ExcelService(IProductRepository productRepository) : IExcelService
{
    [Obsolete("Obsolete")]
    public async Task<int> ImportProductsAsync(Stream fileStream)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Student");
        int count = 0;

        using (var package = new ExcelPackage(fileStream))
        {
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var name = worksheet.Cells[row, 1].Value?.ToString();
                var priceText = worksheet.Cells[row, 2].Value?.ToString();
                var stockText = worksheet.Cells[row, 3].Value?.ToString();
                var description = worksheet.Cells[row, 4].Value?.ToString();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(priceText)) continue;
                
                if (await productRepository.ExistsByNameAsync(name))
                {
                    continue; 
                }
           
                decimal.TryParse(priceText, out decimal price);
                int.TryParse(stockText, out int stock);

                var product = new Product
                {
                    Name = name,
                    Price = price,
                    Stock = stock,
                    Description = description ?? "",
                    IsActive = true
                };

                await productRepository.AddAsync(product);
                count++;
            }
        }
        return count; 
    }
}