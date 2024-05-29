using Microsoft.EntityFrameworkCore;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Models;

namespace MindfulWalletAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId);

            modelBuilder.Entity<RefreshToken>()
                .ToTable("refreshTokens");
        }
    }
}
