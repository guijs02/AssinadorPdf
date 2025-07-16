using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfProcessorController(IPdfProcessorService _service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Execute(IFormFile pdf, IFormFile signature)
        {
            await _service.Execute(pdf, signature);
            return Ok();
        }
    }
}
