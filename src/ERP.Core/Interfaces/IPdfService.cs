using ERP.Core.Entities;

namespace ERP.Core.Interfaces;

public interface IPdfService
{
    byte[] GenerateSaleReceipt(Sale sale);
}