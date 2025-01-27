using GenerateShortUrls.BLL.Contracts;
using GenerateShortUrls.BLL.RealizationServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenerateShortUrl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateShortUrl([FromBody] string originalUrl)
        {
            Guid? userId = User.Identity.IsAuthenticated
                ? Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value)
                : null;
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                return BadRequest("Original URL cannot be null or empty.");
            }

            try
            {
                var shortUrl = await _urlService.CreateShortUrlAsync(originalUrl, userId);
                return Ok(new { shortUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpGet("user/{userId}/urls")]
        public async Task<IActionResult> GetUserUrls(Guid userId)
        {
            try
            {
                var urls = await _urlService.GetUserUrlsAsync(userId);
                return Ok(urls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            if (string.IsNullOrWhiteSpace(shortCode))
            {
                return BadRequest("Short code cannot be null or empty.");
            }

            var shortUrl = $"https://localhost:7107/{shortCode}";

            try
            {
                var originalUrl = await _urlService.GetOriginalUrlAsync(shortUrl);
                return Ok(originalUrl);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Short URL not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
