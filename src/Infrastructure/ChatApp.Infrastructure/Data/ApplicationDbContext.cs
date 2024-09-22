using ChatApp.Domain.Models.Messages;
using ChatApp.Domain.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserModel>().Property(u => u.Initials).HasMaxLength(5);
        builder.Entity<UserModel>().Property(u => u.Name).HasMaxLength(150);
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<MessageModel> Messages { get; set; }
}
