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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            object value = modelBuilder.Entity<User>().ToTable("users");
        }
    }
}
