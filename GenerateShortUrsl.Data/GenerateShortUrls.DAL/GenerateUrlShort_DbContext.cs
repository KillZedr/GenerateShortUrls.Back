using GenerateShortUrsl.Data.Identity;
using GenerateShortUrsl.Data.Url;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrsl.Data.GenerateShortUrls.DAL
{
    public class GenerateUrlShort_DbContext : DbContext
    {
        public GenerateUrlShort_DbContext(DbContextOptions<GenerateUrlShort_DbContext> options)
           : base(options)
        {
        }

        public DbSet<UrlMapping> UrlMappings { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UrlMapping>(entity =>
            {
                entity.HasKey(e => e.Identifier);

                entity.Property(e => e.ShortenedUrl)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.OriginalUrl)
                      .IsRequired()
                      .HasMaxLength(2048);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.UrlMappings)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Identifier);

                entity.Property(e => e.UserName)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                      .IsRequired();

                entity.HasIndex(e => e.Email) 
                      .IsUnique();

                entity.Property(e => e.Token)
                      .HasMaxLength(512); 
            });
        }
    }
}
