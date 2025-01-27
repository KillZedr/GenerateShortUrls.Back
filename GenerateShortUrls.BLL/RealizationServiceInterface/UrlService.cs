using GenerateShortUrls.BLL.Contracts;
using GenerateShortUrsl.Data.GenerateShortUrls.DAL.Contracts;
using GenerateShortUrsl.Data.Identity;
using GenerateShortUrsl.Data.Url;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrls.BLL.RealizationServiceInterface
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository<UrlMapping> _urlRepository;
        private readonly IUrlRepository<User> _userRepository;

        public UrlService(IUrlRepository<UrlMapping> urlRepository, IUrlRepository<User> userRepository)
        {
            _urlRepository = urlRepository;
            _userRepository = userRepository;
        }

        public async Task<string> CreateShortUrlAsync(string originalUrl, Guid? userId = null)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                throw new ArgumentException("Original URL cannot be null or empty.");
            }

            
            if (userId.HasValue)
            {
                var userExists = await _userRepository.AsQueryable().AnyAsync(u => u.Identifier == userId.Value);
                if (!userExists)
                {
                    throw new ArgumentException("User does not exist.");
                }
            }

            
            var existingUrl = await _urlRepository.AsQueryable()
                                                  .FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl && u.UserId == userId);
            if (existingUrl != null)
            {
                return existingUrl.ShortenedUrl;
            }

            
            var shortCode = GenerateShortCode();
            var shortenedUrl = $"https://localhost:7107/{shortCode}";

            // Создаем новую запись
            var urlMapping = new UrlMapping
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortenedUrl,
                UserId = userId
            };

            _urlRepository.Create(urlMapping);
            await _urlRepository.SaveChangesAsync();

            return shortenedUrl;
        }
        public async Task<List<UrlMapping>> GetUserUrlsAsync(Guid userId)
        {
            return await _urlRepository.AsQueryable()
                                       .Where(u => u.UserId == userId)
                                       .ToListAsync();
        }

        public async Task<string> GetOriginalUrlAsync(string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                throw new ArgumentException("Short URL cannot be null or empty.");
            }

            var urlMapping = await _urlRepository.AsQueryable()
                                                 .FirstOrDefaultAsync(u => u.ShortenedUrl == shortUrl);

            if (urlMapping == null)
            {
                throw new KeyNotFoundException("Short URL not found.");
            }

            return urlMapping.OriginalUrl;
        }

        public async Task<bool> UrlExistsAsync(string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                throw new ArgumentException("Short URL cannot be null or empty.");
            }

            return await _urlRepository.AsQueryable()
                                       .AnyAsync(u => u.ShortenedUrl == shortUrl);
        }


        private string GenerateShortCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Range(0, 8)
                                        .Select(_ => chars[random.Next(chars.Length)])
                                        .ToArray());
        }
    }
}
