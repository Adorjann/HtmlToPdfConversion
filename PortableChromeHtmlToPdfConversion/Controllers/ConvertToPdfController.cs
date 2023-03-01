using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace PortableChromeHtmlToPdfConversion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertToPdfController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GeneratePdf()
        {
            var chromePath = Path.Combine(@"Path-to\chrome.exe");
            var options = new LaunchOptions
            {
                ExecutablePath = chromePath,
                Headless = true,
            };

            using (var browser = await Puppeteer.LaunchAsync(options))
            {
                using var page = await browser.NewPageAsync();
                var path = @"Path-to\Html-Template.html";
                var html = await System.IO.File.ReadAllTextAsync(path);

                await page.SetContentAsync(html);

                var stream = new MemoryStream();

                var pdfStream = await page.PdfStreamAsync();
                pdfStream.CopyTo(stream);

                return File(stream.ToArray(), "application/pdf", "Document-Name.pdf");
            }
        }
    }
}
