using System.ComponentModel.DataAnnotations;

namespace Identity.WebApp.Models;

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}