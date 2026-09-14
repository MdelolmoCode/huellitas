using System.ComponentModel.DataAnnotations;

namespace Huellitas.ViewModels.Account;

public sealed class LoginViewModel
{
    [Display(Name = "Nombre de usuario")]
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recuérdame")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
