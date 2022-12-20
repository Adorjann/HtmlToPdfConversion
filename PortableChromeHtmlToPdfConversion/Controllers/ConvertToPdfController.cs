using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            var chromePath = Path.Combine(@"C:\Users\AdorijanCizmar\Desktop\PortableChrome\App\Chrome-bin\chrome.exe");
            var options = new LaunchOptions
            {
                ExecutablePath = chromePath,
                Headless = true,
            };

            using (var browser = await Puppeteer.LaunchAsync(options))
            {
                using var page = await browser.NewPageAsync();
                var path = @"C:\Users\AdorijanCizmar\Documents\Repos\PortableChromeHtmlToPdfConversion\PortableChromeHtmlToPdfConversion\PortableChromeHtmlToPdfConversion\HtmlTemplate\CustomHtml(1).html";
                var html = await System.IO.File.ReadAllTextAsync(path);

                await page.SetContentAsync(html);

                var stream = new MemoryStream();

                var pdfStream = await page.PdfStreamAsync();
                pdfStream.CopyTo(stream);

                return File(stream.ToArray(), "application/pdf", "ChromePrintTest.pdf");
            }
        }
    }
}
