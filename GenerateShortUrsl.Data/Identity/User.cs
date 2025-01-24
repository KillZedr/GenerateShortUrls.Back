using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateShortUrsl.Data.Url;

namespace GenerateShortUrsl.Data.Identity
{
    public class User : Entity<int>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!; 
        public string Email { get; set; } = null!;  
        public string PasswordHash { get; set; } = null!; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

      
        public virtual ICollection<UrlMapping> UrlMappings { get; set; } = new List<UrlMapping>();
    }
}
