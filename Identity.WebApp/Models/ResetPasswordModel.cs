using System.ComponentModel.DataAnnotations;

namespace Identity.WebApp.Models;

public class ResetPasswordModel
{
    public string Token { get; set; }

    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}