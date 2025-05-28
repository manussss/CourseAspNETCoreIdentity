using Microsoft.AspNetCore.Identity;

namespace Identity.WebApp.Models;

public class User : IdentityUser
{
    public string OrganizationId { get; set; }
    public string CompleteName { get; set; }
}