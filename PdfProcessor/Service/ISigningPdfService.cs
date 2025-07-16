namespace PdfProcessor.Service
{
    public interface ISigningPdfService
    {
        public Task<byte[]> SignPdfAsync(MultipartFormDataContent content);
    }
}
