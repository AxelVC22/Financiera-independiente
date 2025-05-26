using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = iTextSharp.text.Paragraph;

namespace Independiente.Services
{
    public interface ICreditApplicationGeneratorService
    {
        void GenerateCompleteReport(string outputPath, Model.Client client, Model.CreditApplication application, List<Model.AmortizationSchedule> schedule);
    }
    public class CreditApplicationGeneratorService : ICreditApplicationGeneratorService
    {
        public void GenerateCompleteReport(string outputPath, Model.Client client, Model.CreditApplication application, List<Model.AmortizationSchedule> schedule)
        {
            Document document = new Document(PageSize.A4, 40, 40, 80, 50);

            using (PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create)))
            {
                writer.PageEvent = new PdfPageEventHelper();

                document.Open();

                AddHeader(document);
                AddClientInformationTable(document, client);
                AddWorkCenterTable(document, client.WorkCenter);
                AddReferencesTable(document, client.FirstReference, client.SecondReference);
                AddCreditDetailsTable(document, application);
                AddAmortizationTable(document, schedule);
                AddSignatureSection(document);

                document.Close();
            }
        }
        private void AddHeader(Document document)
        {
            string logoPath = "logo.png";
            if (File.Exists(logoPath))
            {
                Image logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(150f, 150f);
                logo.Alignment = Image.ALIGN_CENTER;
                document.Add(logo);
            }

            Paragraph title = new Paragraph("SOLICITUD DE CREDITO", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD, BaseColor.DARK_GRAY));
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            document.Add(title);
        }

        private void AddClientInformationTable(Document document, Model.Client client)
        {
            PdfPTable table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f
            };

            table.AddCell(CreateHeader("Información personal", 2));

            table.AddCell(CreateCell("Nombre:", Font.BOLD));
            table.AddCell(CreateCell($"{client.PersonalData.Name} {client.PersonalData.LastName} {client.PersonalData.Surname}"));

            table.AddCell(CreateCell("Fecha de nacimiento:", Font.BOLD));
            table.AddCell(CreateCell(client.PersonalData.BirthDate.HasValue ? client.PersonalData.BirthDate.Value.ToString("yyyy-MM-dd") : ""));


            table.AddCell(CreateCell("RFC:", Font.BOLD));
            table.AddCell(CreateCell(client.PersonalData.RFC));

            table.AddCell(CreateCell("CURP:", Font.BOLD));
            table.AddCell(CreateCell(client.PersonalData.CURP));

            table.AddCell(CreateCell("Número telefónico:", Font.BOLD));
            table.AddCell(CreateCell(client.PersonalData.PhoneNumber));

            table.AddCell(CreateCell("Correo electrónico:", Font.BOLD));
            table.AddCell(CreateCell(client.PersonalData.Email));

            table.AddCell(CreateCell("Domicilio:", Font.BOLD));
            table.AddCell(CreateCell($"{client.AddressData.Street},{client.AddressData.NeighborHood}, {client.AddressData.City}, {client.AddressData.State}"));

            document.Add(table);
        }

        private void AddWorkCenterTable(Document document, Model.WorkCenter workCenter)
        {
            if (workCenter == null) return;

            PdfPTable table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f
            };

            table.AddCell(CreateHeader("Centro de trabajo", 2));

            table.AddCell(CreateCell("Nombre:", Font.BOLD));
            table.AddCell(CreateCell(workCenter.Name));

            table.AddCell(CreateCell("Puesto:", Font.BOLD));
            table.AddCell(CreateCell(workCenter.Role));

            table.AddCell(CreateCell("Fecha de contratación:", Font.BOLD));
            table.AddCell(CreateCell(workCenter.HiringDate.HasValue ? workCenter.HiringDate.Value.ToString("yyyy-MM-dd") : ""));

            table.AddCell(CreateCell("Ingreso mensual:", Font.BOLD));
            table.AddCell(CreateCell(workCenter.MontlyIncome.ToString()));

            document.Add(table);
        }

        private void AddReferencesTable(Document document, Model.Reference firstReference, Model.Reference secondReference)
        {
            if (firstReference == null || secondReference == null) return;

            PdfPTable table = new PdfPTable(5)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f
            };
            table.SetWidths(new float[] { 2f, 2f, 2f, 2f, 2f });

            table.AddCell(CreateHeader("Contactos de referencia", 5));

            string[] headers = { "Nombre", "Apellidos", "Número telefónico", "Correo electrónico", "Parentesco" };
            foreach (string header in headers)
            {
                table.AddCell(CreateCell(header, Font.BOLD, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY));
            }

            table.AddCell(CreateCell(firstReference.Name));
            table.AddCell(CreateCell(firstReference.FullLastName));
            table.AddCell(CreateCell(firstReference.PhoneNumber));
            table.AddCell(CreateCell(firstReference.Email));
            table.AddCell(CreateCell(firstReference.Relationship));

            table.AddCell(CreateCell(secondReference.Name));
            table.AddCell(CreateCell(secondReference.FullLastName));
            table.AddCell(CreateCell(secondReference.PhoneNumber));
            table.AddCell(CreateCell(secondReference.Email));
            table.AddCell(CreateCell(secondReference.Relationship));

            document.Add(table);
        }

        private void AddCreditDetailsTable(Document document, Model.CreditApplication application)
        {
            PdfPTable table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f
            };

            table.AddCell(CreateHeader("Detalles del crédito", 2));

            table.AddCell(CreateCell("Monto solicitado:", Font.BOLD));
            table.AddCell(CreateCell(application.LoanAmount.ToString()));

            table.AddCell(CreateCell("Fecha de solicitud:", Font.BOLD));
            table.AddCell(CreateCell(application.LoanApplicationDate.ToString("yyyy-MM-dd")));

            //table.AddCell(CreateCell("Promoción:", Font.BOLD));
            //table.AddCell(CreateCell(application.Promotion));

            table.AddCell(CreateCell("Tasa de interés:", Font.BOLD));
            table.AddCell(CreateCell($"{(application.PromotionalOffer.InteresRate)*100}%"));

            table.AddCell(CreateCell("Plazo:", Font.BOLD));
            table.AddCell(CreateCell(application.PromotionalOffer.LoanTerm.ToString()));

            table.AddCell(CreateCell("Periodicidad:", Font.BOLD));
            table.AddCell(CreateCell(application.PromotionalOffer.PaymenteFrecuency));

            table.AddCell(CreateCell("IVA:", Font.BOLD));
            table.AddCell(CreateCell($"{(application.PromotionalOffer.IVA)*100}%"));

            document.Add(table);
        }

        private void AddAmortizationTable(Document document, List<Model.AmortizationSchedule> schedule)
        {
            if (schedule == null || !schedule.Any()) return;

            PdfPTable table = new PdfPTable(5)
            {
                WidthPercentage = 100,
                SpacingBefore = 20f
            };
            table.SetWidths(new float[] { 1f, 1.5f, 2f, 2f, 1.5f });

            table.AddCell(CreateHeader("Tabla de amortización", 6));

            string[] headers = { "No. Pago", "Fecha de pago", "Pago fijo", "Monto restante", "Estado" };
            foreach (string header in headers)
            {
                table.AddCell(CreateCell(header, Font.BOLD, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY));
            }

            foreach (var item in schedule)
            {
                table.AddCell(CreateCell(item.PaymentNumber.ToString(), alignment: Element.ALIGN_CENTER));
                table.AddCell(CreateCell(item.PaymentDate.ToString("yyyy-MM-dd"), alignment: Element.ALIGN_CENTER));
                table.AddCell(CreateCell(item.FixedPayment.ToString("C2"), alignment: Element.ALIGN_RIGHT));
                table.AddCell(CreateCell(item.OutstandingBalance.ToString("C2"), alignment: Element.ALIGN_RIGHT));

                BaseColor statusColor = item.Status == "Pagado" ? BaseColor.GREEN : BaseColor.BLACK;
                table.AddCell(CreateCell(item.Status, alignment: Element.ALIGN_CENTER, color: statusColor));
            }

            document.Add(table);
        }

        private void AddSignatureSection(Document document)
        {
            document.Add(new Paragraph(" ") { SpacingBefore = 40f });

            PdfPTable signatureTable = new PdfPTable(2);
            signatureTable.WidthPercentage = 100;

            PdfPCell clientSignatureCell = new PdfPCell();
            clientSignatureCell.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, -1))));
            clientSignatureCell.AddElement(new Paragraph("Firma del cliente",
                new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK))
            { Alignment = Element.ALIGN_LEFT });

            PdfPCell bankSignatureCell = new PdfPCell();
            bankSignatureCell.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, -1))));
            bankSignatureCell.AddElement(new Paragraph("Firma del asesor",
                new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK))
            { Alignment = Element.ALIGN_LEFT });

            signatureTable.AddCell(clientSignatureCell);
            signatureTable.AddCell(bankSignatureCell);

            document.Add(signatureTable);

            Paragraph date = new Paragraph("Fecha: ___________________________",
                new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
            date.Alignment = Element.ALIGN_RIGHT;
            date.SpacingBefore = 20f;
            document.Add(date);
        }

        private PdfPCell CreateCell(string text, int style = Font.NORMAL, int alignment = Element.ALIGN_LEFT, BaseColor color = null)
        {
            Font font = new Font(Font.FontFamily.HELVETICA, 10, style, color ?? BaseColor.BLACK);
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = alignment;
            cell.Padding = 5f;
            cell.BorderWidth = 0.5f;
            return cell;
        }

        private PdfPCell CreateHeader(string text, int colspan)
        {
            PdfPCell headerCell = new PdfPCell(new Phrase(text, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)));
            headerCell.BackgroundColor = new BaseColor(0, 51, 102);
            headerCell.Colspan = colspan;
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            return headerCell;
        }
    }
}
