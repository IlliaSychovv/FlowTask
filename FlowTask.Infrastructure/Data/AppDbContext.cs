using FlowTask.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowTask.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TaskItem>()
            .Property(t => t.Status)
            .HasConversion<string>();
        
        builder.Entity<TaskItem>()
            .Property(t => t.Priority)
            .HasConversion<string>();

        builder.Entity<TaskItem>()
            .HasIndex(i => i.Id);
    }
    
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
}