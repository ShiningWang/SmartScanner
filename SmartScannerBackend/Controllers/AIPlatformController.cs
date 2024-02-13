using Microsoft.AspNetCore.Mvc;
using SmartScannerBackend.DataAccess;
using SmartScannerBackend.Services.AzureAI;
using SmartScannerBackend.Services.OpenAI;

namespace SmartScannerBackend.Controllers
{
    [ApiController]
    [Route("api/AIPlatform")]
    public class AIPlatformController : ControllerBase
    {
        private readonly SmartScannerDbContext _dbContext;

        private IContentExtraction _contentExtraction;

        private IResolveContent _resolveContent;

        public AIPlatformController(SmartScannerDbContext dbContext, IContentExtraction contentExtraction, IResolveContent resolveContent)
        {
            _dbContext = dbContext;
            _contentExtraction = contentExtraction;
            _resolveContent = resolveContent;
        }

        [Route("ReadImage")]
        [HttpPost]
        public async Task<IActionResult> ReadImage(IFormFile img)
        {
            long imageLength;

            using (var stream = img.OpenReadStream())
            {
                stream.Seek(0, SeekOrigin.Begin);
                imageLength = await _contentExtraction.SetImage(stream);
            };

            if (imageLength <= 500000)
            {
                return new BadRequestObjectResult(new { success = false, error = "Image invalid" });
            };

            await _contentExtraction.Resolve();
            string textResolved = _contentExtraction.ReadFullContent();

            if (textResolved.Length <= 10)
            {
                return new BadRequestObjectResult(new { success = false, error = "Failed to resolve the image" });
            };

            await _resolveContent.Decode(textResolved);

            return new OkObjectResult(new { success = true, payload = new { text = textResolved } });
        }
    }
}