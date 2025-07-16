using iText.IO.Image;
using iText.Kernel.Pdf;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PdfProcessor.Core.Entity;
using PdfProcessor.Core.Events;
using PdfProcessor.Core.Repository;
using WebApi.HubSignalR;
using System.Drawing;
using System.Drawing.Imaging;

namespace Worker.Processor.Consumer
{
    public class PdfFileRequestedEventConsumer(
                                IPdfProcessorRepository pdfProcessorRepository,
                                IHubContext<ProcessorPdfHub> hubContext) : IConsumer<SignatureDocumentRequestEvent>
    {
        public async Task Consume(ConsumeContext<SignatureDocumentRequestEvent> context)
        {
            try
            {

                if (context.Message.pdfName is null || context.Message.pdfBytes is null)
                {
                    throw new ArgumentNullException("File name or form file cannot be null.");
                }
                if (context.Message.pdfBytes.Length < 5 ||
                    !System.Text.Encoding.ASCII.GetString(context.Message.pdfBytes, 0, 5).StartsWith("%PDF-"))
                    throw new InvalidDataException("Arquivo PDF inválido ou corrompido.");

                var extension = Path.GetExtension(context.Message.signatureName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    throw new InvalidOperationException("Formato de imagem não suportado.");
                }

                // Caminho para salvar o PDF assinado
                var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");
                var outputDirSignature = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

                Directory.CreateDirectory(outputDir);
                Directory.CreateDirectory(outputDirSignature);

                var outputFileName = $"{Guid.NewGuid()}_{context.Message.pdfName}";
                var outputSignature = $"{Guid.NewGuid()}{context.Message.signatureName}";
                var outputPath = Path.Combine(outputDir, outputFileName);
                var outputPathSignature = Path.Combine(outputDirSignature, outputSignature);

                // Processa o PDF com iText
                using var inputStream = new MemoryStream(context.Message.pdfBytes);

                using var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);

                await File.WriteAllBytesAsync(outputPathSignature, context.Message.signatureBytes);

                var reader = new PdfReader(inputStream);
                var writer = new PdfWriter(outputStream);
                var pdfDoc = new PdfDocument(reader, writer);
                var doc = new iText.Layout.Document(pdfDoc);

                var pdfPage = pdfDoc.GetFirstPage();
                var pageSize = pdfPage.GetPageSize();

                // Largura e altura da imagem
                float imageWidth = 100;
                float imageHeight = 100;

                // Posição X: margem direita
                float x = pageSize.GetRight() - imageWidth - 20; // 20 = margem direita

                // Posição Y: margem inferior (rodapé)
                float y = pageSize.GetBottom() + 20; // 20 = margem inferior

                var imageBytes = File.ReadAllBytes(outputPathSignature);
                var imageData = ImageDataFactory.Create(imageBytes);
                var image = new iText.Layout.Element.Image(imageData)
                    .ScaleToFit(imageWidth, imageHeight)
                    .SetFixedPosition(1, x, y); // Página 1, posição X/Y

                doc.Add(image);
                doc.Close();

                PdfFile pdfFile = new PdfFile(context.Message.id, outputPath, outputFileName, EPdfFileStatus.Completed);

                await pdfProcessorRepository.SaveFile(pdfFile);

                // Obtém a variável de ambiente
                var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://localhost:7167";

                // Separa múltiplas URLs e pega apenas a que começa com https
                var baseUrl = urls
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault(u => u.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    ?? "https://localhost:7167";

                var pdfUrl = $"{baseUrl}/pdfs/{outputFileName}";

                await hubContext.Clients.All.SendAsync("PdfFileProcessed", pdfUrl);
            }
            catch (Exception ex)
            {
                PdfFile pdfFile = new PdfFile(context.Message.id, string.Empty, context.Message.pdfName, EPdfFileStatus.Failed);

                await pdfProcessorRepository.SaveFile(pdfFile);

                Console.WriteLine($"General Exception: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");

                await hubContext.Clients.All.SendAsync("PdfFileProcessedFailed");
                throw;
            }
        }
    }
}
