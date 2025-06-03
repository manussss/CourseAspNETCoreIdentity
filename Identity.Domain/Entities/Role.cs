using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class Role : IdentityRole<int>
{
    public List<UserRole> UserRoles { get; set; }
}