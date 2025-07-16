using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using PdfProcessor.Service;

namespace Front_Web.Pages
{
    public partial class Index : ComponentBase
    {
        public IBrowserFile? selectedPdfFile;
        public IBrowserFile? selectedSignature;
        public bool isFileSelected = false;
        public byte[]? fileContent;
        private string? signaturePreviewUrl;
        public ResultProcess resultProcess = new();
        private bool isSending = false;
        private HubConnection? hubConnection;

        [Inject]
        private ISigningPdfService _signingPdfService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            resultProcess.PdfUrl = string.Empty;
            resultProcess.Name = string.Empty;

            const string hubUrl = "https://localhost:7167/ProcessorHub";
            hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<string?>("PdfFileProcessed", PdfFileProcessed);
            hubConnection.On("PdfFileProcessedFailed", PdfFileProcessedFailed);

            await hubConnection.StartAsync();
        }

        private void HandlePdfSelected(InputFileChangeEventArgs e)
        {
            selectedPdfFile = e.File;
        }
        private async Task HandleSignatureSelected(InputFileChangeEventArgs e)
        {
            selectedSignature = e.File;

            if (selectedSignature != null)
            {
                // Converte imagem em base64 para preview
                var buffer = new byte[selectedSignature.Size];
                await selectedSignature.OpenReadStream().ReadAsync(buffer);
                var base64 = Convert.ToBase64String(buffer);
                signaturePreviewUrl = $"data:{selectedSignature.ContentType};base64,{base64}";
            }
        }
        private void ClearSignature()
        {
            selectedSignature = null;
            signaturePreviewUrl = null;

            StateHasChanged();
        }
        private void PdfFileProcessed(string? url)
        {
            resultProcess.PdfUrl = url;
            resultProcess.Name = selectedPdfFile == null || selectedPdfFile.Name == null ? string.Empty : selectedPdfFile.Name;
            resultProcess.Status = "Sucesso";

            StateHasChanged();
            Console.WriteLine($"Mensagem recebida: {url}");
        }
        private void PdfFileProcessedFailed()
        {
            resultProcess.Name = selectedPdfFile == null || selectedPdfFile.Name == null ? string.Empty : selectedPdfFile.Name;
            resultProcess.Status = "Falha";

            StateHasChanged();
        }
        private async Task SendArchive()
        {
            if (selectedPdfFile is null || selectedSignature is null)
            {
                return;
            }

            isSending = true;

            resultProcess.Status = "Em andamento";

            using var content = new MultipartFormDataContent();
            ////var fileContent = new StreamContent(selectedPdfFile.OpenReadStream());
            ////fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            //content.Add(new StreamContent(selectedPdfFile.OpenReadStream()), "pdf", selectedPdfFile.Name);
            //content.Add(new StreamContent(selectedSignature.OpenReadStream()), "signature", selectedSignature.Name);

            // PDF
            var pdfContent = new StreamContent(selectedPdfFile.OpenReadStream(10 * 1024 * 1024)); // limite de 10MB
            pdfContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            content.Add(pdfContent, "pdf", selectedPdfFile.Name);

            // Assinatura (imagem)
            var assinaturaContent = new StreamContent(selectedSignature.OpenReadStream(5 * 1024 * 1024)); // limite de 5MB
            assinaturaContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(selectedSignature.ContentType);
            content.Add(assinaturaContent, "signature", selectedSignature.Name);

            await _signingPdfService.SignPdfAsync(content);

            isSending = false;
        }

        public class ResultProcess
        {
            public string Name { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string PdfUrl { get; set; } = string.Empty;
        }
    }
}
