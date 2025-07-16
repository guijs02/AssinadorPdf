using PdfProcessor.Core.Entity;
using PdfProcessor.Core.Repository;
using ProcessorPdf.Infra.Data;
using ProcessorPdf.Infra.Model;

namespace ProcessorPdf.Infra.Persistence
{
    public class PdfProcessorRepository(ProcessorPdfDbContext context) : IPdfProcessorRepository
    {
        public async Task SaveFile(PdfFile pdfFile)
        {
            var existingFile = await context.PdfFile.FindAsync(pdfFile.Id);

            if (existingFile is null)
            {
                throw new Exception($"Pdf file with ID {pdfFile.Id} not found.");
            }

            existingFile.Path = pdfFile.Path;
            existingFile.ProcessedAt = pdfFile.ProcessedAt;
            existingFile.Status = (Model.EPdfFileStatus)pdfFile.Status;

            await context.SaveChangesAsync();
        }

        public async Task SaveStatus(PdfFile pdfFile)
        {
            var model = new PdfFileModel
            {
                Id = pdfFile.Id,
                Name = pdfFile.Name,
                Status = (Model.EPdfFileStatus)pdfFile.Status,
                ProcessedAt = pdfFile.ProcessedAt,
            };

            context.PdfFile.Add(model);
            await context.SaveChangesAsync();
        }
    }
}
