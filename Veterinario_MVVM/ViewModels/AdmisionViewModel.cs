using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Veterinario_MVVM.Models;
using Veterinario_MVVM.Services;
using Veterinario_MVVM.ViewModels.Base;

namespace Veterinario_MVVM.ViewModels
{
    public class AdmisionViewModel : ValidatableViewModel
    {
        private readonly ReiacService _reiacService;

        private string _microchip = string.Empty;
        public string Microchip
        {
            get => _microchip;
            set
            {
                if (SetProperty(ref _microchip, value))
                {
                    ValidateProperty(value);
                    _ = ValidarMicrochipAsincronoAsync(value);
                }
            }
        }

        private string _nombreMascota = string.Empty;
        public string NombreMascota
        {
            get => _nombreMascota;
            set
            {
                if (SetProperty(ref _nombreMascota, value))
                    ValidateProperty(value);
            }
        }

        private string _especie = "Perro";
        public string Especie
        {
            get => _especie;
            set
            {
                if (SetProperty(ref _especie, value))
                {
                    ValidateProperty(value);
                    ValidarReglasCruzadasBiometricas();
                }
            }
        }

        private string _raza = string.Empty;
        public string Raza
        {
            get => _raza;
            set
            {
                if (SetProperty(ref _raza, value))
                    ValidateProperty(value);
            }
        }

        private DateTime _fechaNacimiento = DateTime.Today.AddYears(-2);
        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                if (SetProperty(ref _fechaNacimiento, value))
                {
                    ValidateProperty(value);
                    ValidarFechaNacimiento(value);
                }
            }
        }

        private double _peso = 10.0;
        public double Peso
        {
            get => _peso;
            set
            {
                if (SetProperty(ref _peso, value))
                {
                    ValidateProperty(value);
                    ValidarReglasCruzadasBiometricas();
                }
            }
        }

        private double _temperatura = 38.5;
        public double Temperatura
        {
            get => _temperatura;
            set
            {
                if (SetProperty(ref _temperatura, value))
                    ValidateProperty(value);
            }
        }

        private string _titularDni = string.Empty;
        public string TitularDni
        {
            get => _titularDni;
            set
            {
                if (SetProperty(ref _titularDni, value))
                {
                    ValidateProperty(value);
                    ValidarDniModulo23(value);
                }
            }
        }

        private string _titularNombre = string.Empty;
        public string TitularNombre
        {
            get => _titularNombre;
            set
            {
                if (SetProperty(ref _titularNombre, value))
                    ValidateProperty(value);
            }
        }

        private string _titularTelefono = string.Empty;
        public string TitularTelefono
        {
            get => _titularTelefono;
            set
            {
                if (SetProperty(ref _titularTelefono, value))
                    ValidateProperty(value);
            }
        }

        private string _titularEmail = string.Empty;
        public string TitularEmail
        {
            get => _titularEmail;
            set
            {
                if (SetProperty(ref _titularEmail, value))
                    ValidateProperty(value);
            }
        }

        private bool _requiereCirugia;
        public bool RequiereCirugia
        {
            get => _requiereCirugia;
            set
            {
                if (SetProperty(ref _requiereCirugia, value))
                    ValidarProtocoloQuirurgico();
            }
        }

        private int? _horasAyuno;
        public int? HorasAyuno
        {
            get => _horasAyuno;
            set
            {
                if (SetProperty(ref _horasAyuno, value))
                    ValidarProtocoloQuirurgico();
            }
        }

        private string? _tipoIntervencion;
        public string? TipoIntervencion
        {
            get => _tipoIntervencion;
            set
            {
                if (SetProperty(ref _tipoIntervencion, value))
                    ValidarProtocoloQuirurgico();
            }
        }

        private bool _consentimientoFirmado;
        public bool ConsentimientoFirmado
        {
            get => _consentimientoFirmado;
            set
            {
                if (SetProperty(ref _consentimientoFirmado, value))
                    ValidarProtocoloQuirurgico();
            }
        }

        private string _observaciones = string.Empty;
        public string Observaciones
        {
            get => _observaciones;
            set
            {
                if (SetProperty(ref _observaciones, value))
                    ValidateProperty(value);
            }
        }

        private bool _isVerificandoMicrochip;
        public bool IsVerificandoMicrochip
        {
            get => _isVerificandoMicrochip;
            set => SetProperty(ref _isVerificandoMicrochip, value);
        }

        private string _mensajeEstado = "Esperando datos de admisión...";
        public string MensajeEstado
        {
            get => _mensajeEstado;
            set => SetProperty(ref _mensajeEstado, value);
        }

        public ICommand RegistrarAdmisionCommand { get; }
        public ICommand LimpiarCommand { get; }
        public ICommand CargarCasoInvalidoCommand { get; }
        public ICommand CargarCasoValidoCommand { get; }

        public AdmisionViewModel()
        {
            _reiacService = new ReiacService();

            RegistrarAdmisionCommand = new RelayCommand(_ => PerformRegistrarAdmision(), _ => CanRegistrarAdmision());
            LimpiarCommand = new RelayCommand(_ => PerformLimpiar());
            CargarCasoInvalidoCommand = new RelayCommand(_ => PerformCargarCasoInvalido());
            CargarCasoValidoCommand = new RelayCommand(_ => PerformCargarCasoValido());
        }

        private async Task ValidarMicrochipAsincronoAsync(string microchip)
        {
            if (string.IsNullOrWhiteSpace(microchip) || microchip.Length != 15 || !Regex.IsMatch(microchip, @"^\d{15}$"))
                return;

            IsVerificandoMicrochip = true;
            try
            {
                bool existe = await _reiacService.ExisteMicrochipAsync(microchip);
                if (existe)
                {
                    AddError(nameof(Microchip), "El microchip ya consta registrado en REIAC a nombre de otro titular.");
                }
            }
            finally
            {
                IsVerificandoMicrochip = false;
            }
        }

        private void ValidarDniModulo23(string dniNie)
        {
            if (string.IsNullOrWhiteSpace(dniNie)) return;

            string valor = dniNie.Trim().ToUpper();
            if (!Regex.IsMatch(valor, @"^[0-9]{8}[A-Z]$|^[XYZ][0-9]{7}[A-Z]$")) return;

            char letraIntroducida = valor[^1];
            string parteNumerica = valor[..^1];

            if (parteNumerica.StartsWith('X')) parteNumerica = "0" + parteNumerica[1..];
            else if (parteNumerica.StartsWith('Y')) parteNumerica = "1" + parteNumerica[1..];
            else if (parteNumerica.StartsWith('Z')) parteNumerica = "2" + parteNumerica[1..];

            if (long.TryParse(parteNumerica, out long numero))
            {
                const string letrasModulo23 = "TRWAGMYFPDXBNJZSQVHLCKE";
                char letraCalculada = letrasModulo23[(int)(numero % 23)];

                if (letraIntroducida != letraCalculada)
                {
                    AddError(nameof(TitularDni), $"DNI/NIE incorrecto. La letra esperada para el número ingresado es '{letraCalculada}'.");
                }
            }
        }

        private void ValidarFechaNacimiento(DateTime fecha)
        {
            if (fecha > DateTime.Today)
            {
                AddError(nameof(FechaNacimiento), "La fecha de nacimiento no puede ser futura.");
            }
            else if (fecha < DateTime.Today.AddYears(-30))
            {
                AddError(nameof(FechaNacimiento), "La fecha no puede superar los 30 años de antigüedad.");
            }
        }

        private void ValidarReglasCruzadasBiometricas()
        {
            ClearErrors(nameof(Peso));

            if (Especie == "Ave" && Peso > 5.0)
            {
                AddError(nameof(Peso), "Incoherencia biométrica: El peso máximo para especies aviares es 5.0 kg.");
            }
            else if (Especie == "Exótico" && Peso > 3.0)
            {
                AddError(nameof(Peso), "Incoherencia biométrica: El peso máximo para pequeñas mascotas/roedores es 3.0 kg.");
            }
        }

        private void ValidarProtocoloQuirurgico()
        {
            ClearErrors(nameof(HorasAyuno));
            ClearErrors(nameof(TipoIntervencion));
            ClearErrors(nameof(ConsentimientoFirmado));

            if (RequiereCirugia)
            {
                if (!HorasAyuno.HasValue || HorasAyuno.Value < 8)
                {
                    AddError(nameof(HorasAyuno), "El protocolo quirúrgico exige un ayuno mínimo de 8 horas.");
                }

                if (string.IsNullOrWhiteSpace(TipoIntervencion))
                {
                    AddError(nameof(TipoIntervencion), "Debe especificar el tipo de intervención o procedimiento.");
                }

                if (!ConsentimientoFirmado)
                {
                    AddError(nameof(ConsentimientoFirmado), "Es obligatorio disponer del consentimiento firmado antes del quirófano.");
                }
            }
        }

        private bool CanRegistrarAdmision()
        {
            return !HasErrors &&
                   !IsVerificandoMicrochip &&
                   !string.IsNullOrWhiteSpace(Microchip) &&
                   !string.IsNullOrWhiteSpace(NombreMascota) &&
                   !string.IsNullOrWhiteSpace(TitularDni);
        }

        private void PerformRegistrarAdmision()
        {
            if (ValidateAll())
            {
                MessageBox.Show($"¡Admisión de '{NombreMascota}' registrada correctamente!",
                                "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                MensajeEstado = $"Paciente {NombreMascota} admitido el {DateTime.Now:dd/MM/yyyy HH:mm}";
                PerformLimpiar();
            }
        }

        private void PerformLimpiar()
        {
            ClearErrors();
            Microchip = string.Empty;
            NombreMascota = string.Empty;
            Especie = "Perro";
            Raza = string.Empty;
            FechaNacimiento = DateTime.Today.AddYears(-2);
            Peso = 10.0;
            Temperatura = 38.5;
            TitularDni = string.Empty;
            TitularNombre = string.Empty;
            TitularTelefono = string.Empty;
            TitularEmail = string.Empty;
            RequiereCirugia = false;
            HorasAyuno = null;
            TipoIntervencion = string.Empty;
            ConsentimientoFirmado = false;
            Observaciones = string.Empty;
            MensajeEstado = "Formulario limpiado.";
        }

        private void PerformCargarCasoInvalido()
        {
            PerformLimpiar();
            Microchip = "999000000000001";
            NombreMascota = "X";
            Especie = "Ave";
            Peso = 12.5;
            Temperatura = 45.0;
            TitularDni = "12345678A";
            TitularTelefono = "12345";
            TitularEmail = "correo-invalido";
            RequiereCirugia = true;
            HorasAyuno = 3;
            ConsentimientoFirmado = false;
            MensajeEstado = "Caso con errores cargado para pruebas visuales.";
        }

        private void PerformCargarCasoValido()
        {
            PerformLimpiar();
            Microchip = "982000123456789";
            NombreMascota = "Thor";
            Especie = "Perro";
            Raza = "Golden Retriever";
            FechaNacimiento = new DateTime(2023, 3, 15);
            Peso = 32.4;
            Temperatura = 38.6;
            TitularDni = "12345678Z";
            TitularNombre = "Laura Gómez Sánchez";
            TitularTelefono = "654987321";
            TitularEmail = "laura.gomez@example.com";
            RequiereCirugia = true;
            HorasAyuno = 12;
            TipoIntervencion = "Limpieza dental profunda";
            ConsentimientoFirmado = true;
            Observaciones = "Paciente en ayunas sin alergias conocidas.";
            MensajeEstado = "Caso válido de prueba cargado correctamente.";
        }
    }
}