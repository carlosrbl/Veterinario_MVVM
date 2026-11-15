using System.ComponentModel.DataAnnotations;

namespace Veterinario_MVVM.Models
{
    /// <summary>
    /// Representa al usuario profesional que accede al sistema de la clínica veterinaria (Entidad ADO).
    /// Utiliza DataAnnotations para definir restricciones, validaciones y metadatos de presentación.
    /// </summary>
    public class Usuario
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 30 caracteres.")]
        [Display(Name = "Nombre de Usuario")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La contraseña debe tener al menos 3 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre completo no puede superar los 80 caracteres.")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol profesional es obligatorio.")]
        [StringLength(50, ErrorMessage = "El rol no puede superar los 50 caracteres.")]
        [Display(Name = "Rol Clínico")]
        public string Rol { get; set; } = "Veterinario/a";
    }
}
