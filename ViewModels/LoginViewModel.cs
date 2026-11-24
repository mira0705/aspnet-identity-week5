using System.ComponentModel.DataAnnotations;

namespace StudentApp.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email енгізіңіз")]
    [EmailAddress(ErrorMessage = "Email форматы дұрыс емес")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпиясөзді енгізіңіз")]
    [DataType(DataType.Password)]
    [Display(Name = "Құпиясөз")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Мені еске сақта")]
    public bool RememberMe { get; set; }
}
