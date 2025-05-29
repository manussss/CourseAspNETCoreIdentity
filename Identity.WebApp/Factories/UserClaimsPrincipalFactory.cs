using System.Security.Claims;
using Identity.WebApp.Factories;
using Identity.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.WebApp.Factories;

public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    public UserClaimsPrincipalFactory(
        UserManager<User> userManager,
        IOptions<IdentityOptions> options
    ) : base(userManager, options)
    {

    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        identity.AddClaim(new Claim("Member", user.Member));

        return identity;
    }
}