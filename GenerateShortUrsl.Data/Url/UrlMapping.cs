using GenerateShortUrsl.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrsl.Data.Url
{
    public class UrlMapping : Entity<int>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
