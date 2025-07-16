namespace PdfProcessor.Core.Events
{
    public sealed record SignatureDocumentRequestEvent(Guid id, string pdfName, byte[] pdfBytes, byte[] signatureBytes, string signatureName)
    {
    }
}
