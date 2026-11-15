using System;
using System.ComponentModel.DataAnnotations;

namespace Veterinario_MVVM.Models
{
    /// <summary>
    /// Representa al paciente atendido en la clínica veterinaria (Entidad ADO).
    /// Incorpora DataAnnotations para metadatos de validación, longitud y rangos de datos clínicos.
    /// </summary>
    public class Mascota
    {
        [Key]
        [Display(Name = "Identificador")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe contener entre 2 y 50 caracteres.")]
        [Display(Name = "Nombre de la Mascota")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especie es obligatoria.")]
        [StringLength(30, ErrorMessage = "La especie no puede superar los 30 caracteres.")]
        [Display(Name = "Especie")]
        public string Especie { get; set; } = "Perro"; // Perro, Gato, Ave, Exótico

        [StringLength(50, ErrorMessage = "La raza no puede superar los 50 caracteres.")]
        [Display(Name = "Raza")]
        public string Raza { get; set; } = string.Empty;

        [Range(0, 40, ErrorMessage = "La edad debe estar comprendida entre 0 y 40 años.")]
        [Display(Name = "Edad (años)")]
        public int Edad { get; set; }

        [Range(0.01, 250.0, ErrorMessage = "El peso debe estar entre 0.01 kg y 250.0 kg.")]
        [Display(Name = "Peso (kg)")]
        public double Peso { get; set; }

        [Required(ErrorMessage = "El nombre del propietario o titular es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre del titular debe tener entre 3 y 80 caracteres.")]
        [Display(Name = "Propietario / Titular")]
        public string Propietario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono de contacto es obligatorio.")]
        [Phone(ErrorMessage = "El formato de teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "El motivo de la consulta no puede superar los 500 caracteres.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Motivo de Consulta")]
        public string MotivoConsulta { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
