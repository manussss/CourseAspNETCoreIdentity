using Microsoft.AspNetCore.Identity;

namespace Identity.WebApp.Validators;

public class PasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
{
    public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string? password)
    {
        var userName = await manager.GetUserNameAsync(user);

        if (userName == password)
            return IdentityResult.Failed(new IdentityError() { Description = "Password must be different from username" });

        return IdentityResult.Success;
    }
}