using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace OMSBlazor.Blazor.Services
{
    public class InvoiceDocument : IDocument
    {
        public InvoiceModel Model { get; }

        public InvoiceDocument(InvoiceModel model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(30);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(25).Column(column =>
            {
                float fontSize = 18;
                column
                    .Item()
                    .AlignCenter()
                    .AlignMiddle()
                    .Text($"Invoice #{Model.InvoiceNumber}")
                    .FontSize(30);

                column.Item().LineHorizontal(1);

                column.Item().AlignRight().Padding(20).Text(text =>
                {
                    text.Span("Order date: ").FontSize(fontSize);
                    text.Span($"{Model.OrderDate}").SemiBold().FontSize(fontSize);
                });

                column.Item().AlignRight().Padding(20).Text(text =>
                {
                    text.Span("Employee: ").FontSize(fontSize);
                    text.Span($"{Model.EmployeeFullName}").SemiBold().FontSize(fontSize);
                });

                column.Item().AlignRight().Padding(20).Text(text =>
                {
                    text.Span("Customer: ").FontSize(fontSize);
                    text.Span($"{Model.CustomerName}").SemiBold().FontSize(fontSize);
                });

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).AlignLeft().Text("Product name");
                        header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                        header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                        header.Cell().Element(CellStyle).AlignRight().Text("Discount");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        }
                    });

                    foreach (var item in Model.Items)
                    {
                        table.Cell().Element(CellStyle).Text(item.ProductName);
                        table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}$");
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.Discount}%");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                        }
                    }
                });

                column.Item().AlignRight().Padding(20).Text(text =>
                {
                    text.Span("Total sum: ").FontSize(fontSize);
                    text.Span($"{Model.TotalPrice} $").SemiBold().FontSize(fontSize);
                });
            });
        }

        private void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.SemiBold();

            container.Row(row =>
            {
                row.Spacing(10);
                //row.RelativeItem().AlignLeft().Image("./northwind-final.png");

                row.RelativeItem().AlignLeft().Text(text =>
                {
                    text.DefaultTextStyle(titleStyle);
                    text.Line("Almazor street, 1-91");
                    text.Line("Tashkent, Uzbekistan");
                    text.Line("Tel.: +998-90-123-45-67");
                });

                row.RelativeItem().AlignRight().Text(text =>
                {
                    text.DefaultTextStyle(titleStyle);
                    text.Line("Created at:");
                    text.Line(DateTime.Now.Date.ToShortDateString());
                    text.Line(DateTime.Now.ToShortTimeString());
                });
            });
        }
    }
}
