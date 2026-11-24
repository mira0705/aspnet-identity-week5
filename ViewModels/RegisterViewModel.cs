using System.ComponentModel.DataAnnotations;

namespace StudentApp.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Атыңызды енгізіңіз")]
    [StringLength(50, ErrorMessage = "Ең көбі 50 таңба")]
    [Display(Name = "Аты")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Тегіңізді енгізіңіз")]
    [StringLength(50, ErrorMessage = "Ең көбі 50 таңба")]
    [Display(Name = "Тегі")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email енгізіңіз")]
    [EmailAddress(ErrorMessage = "Email форматы дұрыс емес")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпиясөзді енгізіңіз")]
    [DataType(DataType.Password)]
    [Display(Name = "Құпиясөз")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпиясөзді растаңыз")]
    [Compare("Password", ErrorMessage = "Құпиясөздер сәйкес емес")]
    [DataType(DataType.Password)]
    [Display(Name = "Құпиясөзді растау")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
