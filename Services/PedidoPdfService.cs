using Pedidos360.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Pedidos360.Services;

// Genera el PDF con el detalle de un pedido 
public static class PedidoPdfService
{
    public static byte[] Generar(Pedido pedido)
    {
        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text("Pedidos360").FontSize(18).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().Text($"Detalle del Pedido #{pedido.Id}").FontSize(13);
                    col.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                });

                page.Content().PaddingVertical(15).Column(col =>
                {
                    col.Spacing(4);

                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Cliente").SemiBold().FontColor(Colors.Grey.Darken1);
                            c.Item().Text($"{pedido.Cliente?.Nombre} {pedido.Cliente?.ApellidoPaterno} {pedido.Cliente?.ApellidoMaterno}");
                            c.Item().Text($"Cédula: {pedido.ClienteId}");
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Pedido").SemiBold().FontColor(Colors.Grey.Darken1);
                            c.Item().Text($"Fecha: {pedido.Fecha:dd/MM/yyyy HH:mm}");
                            c.Item().Text($"Estado: {pedido.Estado?.Descripcion}");
                        });
                    });

                    col.Item().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1.4f);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1.5f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CeldaEncabezado).Text("Producto");
                            header.Cell().Element(CeldaEncabezado).AlignCenter().Text("Cant.");
                            header.Cell().Element(CeldaEncabezado).AlignRight().Text("Precio unit.");
                            header.Cell().Element(CeldaEncabezado).AlignCenter().Text("Desc. %");
                            header.Cell().Element(CeldaEncabezado).AlignCenter().Text("IVA %");
                            header.Cell().Element(CeldaEncabezado).AlignRight().Text("Total línea");

                            static IContainer CeldaEncabezado(IContainer c) =>
                                c.DefaultTextStyle(x => x.SemiBold())
                                 .PaddingVertical(6)
                                 .BorderBottom(1)
                                 .BorderColor(Colors.Grey.Darken1);
                        });

                        foreach (var d in pedido.Detalles)
                        {
                            table.Cell().Element(Celda).Text(d.Producto?.Nombre ?? "—");
                            table.Cell().Element(Celda).AlignCenter().Text(d.Cantidad.ToString());
                            table.Cell().Element(Celda).AlignRight().Text(d.PrecioUnitario.ToString("C"));
                            table.Cell().Element(Celda).AlignCenter().Text($"{d.DescuentoPorc}%");
                            table.Cell().Element(Celda).AlignCenter().Text($"{d.ImpuestoPorc}%");
                            table.Cell().Element(Celda).AlignRight().Text(d.TotalLinea.ToString("C"));

                            static IContainer Celda(IContainer c) =>
                                c.PaddingVertical(5).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2);
                        }
                    });

                    col.Item().PaddingTop(15).AlignRight().Width(220).Column(totales =>
                    {
                        totales.Spacing(3);
                        totales.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Subtotal");
                            r.ConstantItem(100).AlignRight().Text(pedido.Subtotal.ToString("C"));
                        });
                        totales.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Descuento");
                            r.ConstantItem(100).AlignRight().Text($"-{pedido.Descuento:C}");
                        });
                        totales.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Impuesto");
                            r.ConstantItem(100).AlignRight().Text(pedido.Impuesto.ToString("C"));
                        });
                        totales.Item().PaddingTop(4).BorderTop(1).BorderColor(Colors.Grey.Darken1).Row(r =>
                        {
                            r.RelativeItem().Text("Total").Bold().FontSize(12);
                            r.ConstantItem(100).AlignRight().Text(pedido.Total.ToString("C")).Bold().FontSize(12);
                        });
                    });
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Pedidos360 · Generado el ").FontColor(Colors.Grey.Darken1);
                    x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontColor(Colors.Grey.Darken1);
                });
            });
        });

        return documento.GeneratePdf();
    }
}
