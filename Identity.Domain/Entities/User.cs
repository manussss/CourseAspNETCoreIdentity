using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class User : IdentityUser<int>
{
    public string OrganizationId { get; set; }
    public string CompleteName { get; set; }
    public string Member { get; set; } = "Member";
    public List<UserRole> UserRoles { get; set; }
}