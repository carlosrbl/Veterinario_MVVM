using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Veterinario_MVVM.Models
{
    public class AdmisionPaciente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código de microchip es obligatorio.")]
        [RegularExpression(@"^\d{15}$", ErrorMessage = "El microchip debe constar exactamente de 15 dígitos numéricos (ISO 11784/11785).")]
        public string Microchip { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 40 caracteres.")]
        public string NombreMascota { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una especie.")]
        public string Especie { get; set; } = "Perro";

        [Required(ErrorMessage = "La raza es obligatoria.")]
        [StringLength(40, ErrorMessage = "La raza no puede superar los 40 caracteres.")]
        public string Raza { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; } = DateTime.Today.AddYears(-2);

        [Required(ErrorMessage = "El peso es obligatorio.")]
        [Range(0.05, 120.0, ErrorMessage = "El peso debe estar comprendido entre 0.05 kg y 120.0 kg.")]
        public double Peso { get; set; } = 10.0;

        [Required(ErrorMessage = "La temperatura es obligatoria.")]
        [Range(35.0, 42.5, ErrorMessage = "La temperatura corporal debe estar entre 35.0 °C y 42.5 °C.")]
        public double Temperatura { get; set; } = 38.5;

        [Required(ErrorMessage = "El DNI/NIE del titular es obligatorio.")]
        [RegularExpression(@"^[0-9]{8}[A-Z]$|^[XYZ][0-9]{7}[A-Z]$", ErrorMessage = "Formato de DNI/NIE inválido (Ej: 12345678Z o X1234567L).")]
        public string TitularDni { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo del titular es obligatorio.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "El nombre del titular debe tener entre 3 y 60 caracteres.")]
        public string TitularNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono de urgencias es obligatorio.")]
        [RegularExpression(@"^[6789]\d{8}$", ErrorMessage = "El teléfono debe ser un número nacional válido de 9 dígitos.")]
        public string TitularTelefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Introduzca una dirección de correo electrónico válida.")]
        public string TitularEmail { get; set; } = string.Empty;

        public bool RequiereCirugia { get; set; } = false;

        public int? HorasAyuno { get; set; }

        public string? TipoIntervencion { get; set; }

        public bool ConsentimientoFirmado { get; set; } = false;

        [StringLength(250, ErrorMessage = "Las observaciones no pueden superar los 250 caracteres.")]
        public string Observaciones { get; set; } = string.Empty;

        public DateTime FechaAdmision { get; set; } = DateTime.Now;
    }
}
