using System.Net.Http.Json;

namespace PdfProcessor.Service
{
    public class SigningPdfService : ISigningPdfService
    {
        private readonly HttpClient _httpClient;

        public SigningPdfService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient();
        }

        public async Task<byte[]> SignPdfAsync(MultipartFormDataContent content)
        {
            // Implementation for signing PDF goes here

            var response = await _httpClient.PostAsync("api/pdfprocessor/", content);

            return new byte[0]; // Placeholder for the signed PDF byte array
        }
    }
}
