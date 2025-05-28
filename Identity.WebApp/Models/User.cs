using Microsoft.AspNetCore.Identity;

namespace Identity.WebApp.Models;

public class User : IdentityUser
{
    public string CompleteName { get; set; }
}