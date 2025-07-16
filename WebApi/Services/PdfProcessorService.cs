using MassTransit;
using PdfProcessor.Core.Entity;
using PdfProcessor.Core.Events;
using PdfProcessor.Core.Repository;
using System.Text.Json;

namespace WebApi.Services
{
    public class PdfProcessorService(MessageBroker.IPublishMessage bus, IPdfProcessorRepository pdfProcessorRepository) : IPdfProcessorService
    {
        public async Task Execute(IFormFile pdf, IFormFile signature)
        {
            var pdfFile = new PdfFile(Guid.NewGuid(), string.Empty, pdf.FileName, EPdfFileStatus.Pending);

            await pdfProcessorRepository.SaveStatus(pdfFile);

            var pdfBytes = await GetFormFileBytes(pdf);

            var signatureBytes = await GetFormFileBytes(signature);

            var pdfFileEvent = new SignatureDocumentRequestEvent(pdfFile.Id, pdfFile.Name, pdfBytes, signatureBytes, signature.FileName);

            await bus.SendMessage(pdfFileEvent);
        }

        private async Task<byte[]> GetFormFileBytes(IFormFile formFile)
        {
            await using var pdfStream = formFile.OpenReadStream();
            using var ms = new MemoryStream();
            await pdfStream.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
