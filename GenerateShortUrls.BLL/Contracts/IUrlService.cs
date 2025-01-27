using GenerateShortUrsl.Data.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrls.BLL.Contracts
{
    public interface IUrlService : IService
    {
        Task<string> CreateShortUrlAsync(string originalUrl, Guid? userId = null);
        Task<List<UrlMapping>> GetUserUrlsAsync(Guid userId);
        Task<string> GetOriginalUrlAsync(string shortUrl);
        Task<bool> UrlExistsAsync(string shortUrl);
    }
}
