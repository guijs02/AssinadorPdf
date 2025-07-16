namespace WebApi.Services
{
    public interface IPdfProcessorService
    {
        Task Execute (IFormFile pdf, IFormFile signature);
    }
}
