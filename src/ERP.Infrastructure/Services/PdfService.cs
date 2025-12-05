using ERP.Core.Entities;
using ERP.Core.Interfaces;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ERP.Infrastructure.Services;

public class PdfService : IPdfService
{
    public PdfService()
    {
        // Configure QuestPDF Community License (Required for non-commercial use)
        Settings.License = LicenseType.Community;
    }

    public byte[] GenerateSaleReceipt(Sale sale)
    {
        // Generate the PDF document structure
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // -- Page Configuration --
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                // -- 1. Header Section --
                page.Header()
                    .Row(row =>
                    {
                        // Left: Company Title and Sale Metadata
                        row.RelativeItem()
                            .Column(col =>
                            {
                                col.Item()
                                    .Text("ERP System")
                                    .SemiBold()
                                    .FontSize(24)
                                    .FontColor(Colors.Blue.Darken3);
                                col.Item().Text($"Sale Receipt #{sale.Id}").FontSize(14);
                                col.Item().Text($"Date: {sale.Date:yyyy-MM-dd HH:mm}");
                            });

                        // Right: Static Company Information
                        row.ConstantItem(150)
                            .AlignRight()
                            .Column(col =>
                            {
                                col.Item().Text("Construction SL Co.");
                                col.Item().Text("123 Ave Progress.");
                                col.Item().Text("Barranquilla, Colombia");
                                col.Item().Text("NIT: 900.123.456-7");
                            });
                    });

                // -- 2. Main Content --
                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(col =>
                    {
                        // Customer Details Block
                        col.Item()
                            .Container()
                            .Background(Colors.Grey.Lighten4)
                            .Padding(10)
                            .Column(c =>
                            {
                                c.Item().Text("Customer Information").SemiBold();
                                c.Item()
                                    .Text(t =>
                                    {
                                        t.Span("Name: ").SemiBold();
                                        // Use null-conditional operator to handle potential null customer reference
                                        t.Span($"{sale.Customer?.Name} {sale.Customer?.LastName}");
                                    });
                                c.Item()
                                    .Text(t =>
                                    {
                                        t.Span("Document ID: ").SemiBold();
                                        t.Span(sale.Customer?.IdDocument ?? "N/A");
                                    });
                                c.Item()
                                    .Text(t =>
                                    {
                                        t.Span("Email: ").SemiBold();
                                        t.Span(sale.Customer?.Email ?? "N/A");
                                    });
                            });

                        col.Item().PaddingVertical(15); // Spacer

                        // Products Table
                        col.Item()
                            .Table(table =>
                            {
                                // Define Table Columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3); // Product Name (Wider)
                                    columns.RelativeColumn(); // Unit Price
                                    columns.RelativeColumn(); // Quantity
                                    columns.RelativeColumn(); // Line Total
                                });

                                // Table Header Row
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Product Description");
                                    header
                                        .Cell()
                                        .Element(CellStyle)
                                        .AlignRight()
                                        .Text("Unit Price");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("Qty");
                                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                                    // Local function for consistent header styling
                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container
                                            .DefaultTextStyle(x =>
                                                x.SemiBold().FontColor(Colors.White)
                                            )
                                            .Background(Colors.Blue.Darken3)
                                            .PaddingVertical(5)
                                            .PaddingHorizontal(10);
                                    }
                                });

                                // Table Data Rows (Iterate through Sale Details)
                                foreach (var detail in sale.SalesDetails)
                                {
                                    var totalLine = detail.Quantity * detail.UnitPrice;

                                    table
                                        .Cell()
                                        .Element(ItemStyle)
                                        .Text(detail.Product?.Name ?? "Unknown Item");
                                    table
                                        .Cell()
                                        .Element(ItemStyle)
                                        .AlignRight()
                                        .Text($"{detail.UnitPrice:C}");
                                    table
                                        .Cell()
                                        .Element(ItemStyle)
                                        .AlignCenter()
                                        .Text($"{detail.Quantity}");
                                    table
                                        .Cell()
                                        .Element(ItemStyle)
                                        .AlignRight()
                                        .Text($"{totalLine:C}");

                                    // Local function for consistent row styling with bottom border
                                    static IContainer ItemStyle(IContainer container)
                                    {
                                        return container
                                            .BorderBottom(1)
                                            .BorderColor(Colors.Grey.Lighten3)
                                            .PaddingVertical(5)
                                            .PaddingHorizontal(10);
                                    }
                                }
                            });

                        col.Item().PaddingVertical(10); // Spacer

                        // Grand Total Section (Right Aligned)
                        col.Item()
                            .AlignRight()
                            .Row(row =>
                            {
                                row.RelativeItem(); // Empty spacer to push content right
                                row.ConstantItem(200)
                                    .BorderTop(1)
                                    .BorderColor(Colors.Black)
                                    .PaddingTop(5)
                                    .Row(totalRow =>
                                    {
                                        totalRow
                                            .RelativeItem()
                                            .Text("GRAND TOTAL:")
                                            .SemiBold()
                                            .FontSize(14);
                                        totalRow
                                            .RelativeItem()
                                            .AlignRight()
                                            .Text($"{sale.Total:C}")
                                            .SemiBold()
                                            .FontSize(14)
                                            .FontColor(Colors.Green.Darken2);
                                    });
                            });
                    });

                // -- 3. Footer Section --
                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        });

        // Render document to byte array
        return document.GeneratePdf();
    }
}
