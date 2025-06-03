using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infra;

public class WebApiDbContext : IdentityDbContext<User, Role, int,
                                             IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                             IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserRole>(userRole => 
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

            userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
        });

        builder.Entity<Organization>(org =>
        {
            org.ToTable("Organizations");
            org.HasKey(x => x.Id);

            org.HasMany<User>()
                .WithOne()
                .HasForeignKey(x => x.Id)
                .IsRequired(false);
        });
    }
}