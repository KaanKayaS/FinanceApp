using FinanceApp.Application.DTOs;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Interfaces.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class PdfReportService : IPdfReportService
    {
        public byte[] GenerateExpensePdf(IList<GetAllExpenseAndPaymentByUserQueryResult> expenses)
        {
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    // Başlık
                    page.Header().Column(column =>
                    {
                        column.Item().Text("Harcama Raporu")
                            .FontSize(24)
                            .Bold()
                            .AlignCenter();

                        column.Item().LineHorizontal(1)
                            .LineColor(Colors.Grey.Medium);

                        column.Item().PaddingBottom(15);
                    });

                    // Tablo
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Name
                            columns.RelativeColumn(2); // Amount
                            columns.RelativeColumn(3); // PaidDate
                        });

                        // Başlık satırı
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Harcama Adı").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tutar").Bold().AlignRight();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tarih").Bold();
                        });

                        // Satırlar (zebra striping)
                        bool alternate = false;
                        foreach (var expense in expenses)
                        {
                            var background = alternate ? Colors.Grey.Lighten4 : Colors.White;
                            alternate = !alternate;

                            table.Cell().Background(background).Padding(5).Text(expense.Name);
                            table.Cell().Background(background).Padding(5).Text($"{expense.Amount:C}").AlignRight();
                            table.Cell().Background(background).Padding(5).Text(expense.PaidDate.ToString("dd.MM.yyyy HH:mm"));
                        }
                    });

                    // Footer (düzeltilmiş)
                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Rapor Tarihi: ")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        txt.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm"))
                            .SemiBold()
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateInstructionPdf(IList<InstructionDto> instructions)
        {
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    // Başlık
                    page.Header().Column(column =>
                    {
                        column.Item().Text("Talimat Raporu")
                            .FontSize(24)
                            .Bold()
                            .AlignCenter();

                        column.Item().LineHorizontal(1)
                            .LineColor(Colors.Grey.Medium);

                        column.Item().PaddingBottom(15);
                    });

                    // Tablo
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Title
                            columns.RelativeColumn(2); // Amount
                            columns.RelativeColumn(3); // ScheduledDate
                            columns.RelativeColumn(2); // Status
                        });

                        // Başlık satırı
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Talimat Adı").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tutar").Bold().AlignRight();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Planlanan Tarih").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Durum").Bold().AlignCenter();
                        });

                        // Satırlar (ödenmiş / ödenmemiş renkler)
                        bool alternate = false;
                        foreach (var instruction in instructions)
                        {
                            var background = alternate ? Colors.Grey.Lighten4 : Colors.White;
                            alternate = !alternate;

                            table.Cell().Background(background).Padding(5).Text(instruction.Title);
                            table.Cell().Background(background).Padding(5).Text($"{instruction.Amount:C}").AlignRight();
                            table.Cell().Background(background).Padding(5).Text(instruction.ScheduledDate.ToString("dd.MM.yyyy HH:mm"));

                            // Durum rengi
                            var statusColor = instruction.IsPaid ? Colors.Green.Darken1 : Colors.Red.Darken1;
                            var statusText = instruction.IsPaid ? "Ödendi" : "Bekliyor";

                            table.Cell().Background(background).Padding(5)
                                .Text(statusText)
                                .FontColor(statusColor)
                                .AlignCenter();
                        }
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Rapor Tarihi: ")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        txt.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm"))
                            .SemiBold()
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
