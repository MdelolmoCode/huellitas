using System.ComponentModel.DataAnnotations;

namespace Huellitas.ViewModels.Account;

public sealed class RegisterViewModel
{
    [Display(Name = "Nombre de usuario")]
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre de usuario no puede superar los 50 caracteres.")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "Nombre visible")]
    [Required(ErrorMessage = "El nombre visible es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre visible no puede superar los 100 caracteres.")]
    public string DisplayName { get; set; } = string.Empty;

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Escribe un correo electrónico válido.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Repite la contraseña")]
    [Required(ErrorMessage = "Repetir la contraseña es obligatorio.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    public string PasswordConfirmation { get; set; } = string.Empty;
}
