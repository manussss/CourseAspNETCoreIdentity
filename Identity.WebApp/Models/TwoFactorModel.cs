using System.ComponentModel.DataAnnotations;

namespace Identity.WebApp.Models;

public class TwoFactorModel
{
    [Required]
    public string Token { get; set; }
}