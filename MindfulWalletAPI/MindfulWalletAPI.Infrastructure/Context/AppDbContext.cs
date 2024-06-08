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
        public DbSet<ResetToken> ResetTokens { get; set; }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Report> Reports { get; set; }

        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId);

            modelBuilder.Entity<RefreshToken>()
                .ToTable("refreshTokens");

            modelBuilder.Entity<User>()
                .HasMany(u => u.ResetTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId);

            modelBuilder.Entity<ResetToken>()
                .ToTable("resetTokens");

            // Relația 1-1 între User și Finance
            modelBuilder.Entity<User>()
                .HasOne(u => u.Finance)
                .WithOne(f => f.User)
                .HasForeignKey<Finance>(f => f.UserId);

            // Relația 1-N între Finance și Account
            modelBuilder.Entity<Finance>()
                .HasMany(f => f.Accounts)
                .WithOne(a => a.Finance)
                .HasForeignKey(a => a.FinanceId);

            // Relația 1-N între Account și Expense
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Expenses)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);

            //relatie 1-N intre account si report
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Reports)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.AccountId);

            //relatie 1-1 user calendar
            modelBuilder.Entity<User>()
                .HasOne(u => u.Calendar)
                .WithOne(c => c.User)
                .HasForeignKey<Calendar>(c => c.UserId);

            //relatie 1-N user Goal
            modelBuilder.Entity<User>()
                .HasMany(u => u.Goals)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId);

            //relatie 1-N user achievement
            modelBuilder.Entity<User>()
               .HasMany(u => u.Achievements)
               .WithOne(a => a.User)
               .HasForeignKey(a => a.UserId);
            
            //relatie N-1 events calendar
            modelBuilder.Entity<Calendar>()
                .HasMany(c => c.Events)
                .WithOne(e => e.Calendar)
                .HasForeignKey(e => e.CalendarId);
        }
    }
}
