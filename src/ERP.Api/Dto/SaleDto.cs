using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Api.Dto;

public class CreateSaleDto
{
    public List<SaleDetailDto> Details { get; set; } = new();
}

public class SaleDetailDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
