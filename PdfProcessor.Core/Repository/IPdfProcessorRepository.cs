using PdfProcessor.Core.Entity;

namespace PdfProcessor.Core.Repository
{
    public interface IPdfProcessorRepository
    {
        Task SaveStatus(PdfFile pdfFile);
        Task SaveFile(PdfFile pdfFile);
    }
}
