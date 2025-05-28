using Identity.WebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.WebApp.Data;

public class UserDbContext : IdentityDbContext<User>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Organization>(o =>
        {
            o.ToTable("Organizations");
            o.HasKey(x => x.Id);
            o.HasMany<User>()
                .WithOne()
                .HasForeignKey(x => x.OrganizationId)
                .IsRequired(false);
        });

        base.OnModelCreating(builder);
    }
}