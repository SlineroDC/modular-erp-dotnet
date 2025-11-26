namespace ERP.Core.Interfaces;

public interface IExcelService
{
    Task<int> ImportProductsAsync(Stream fileStream);
}