using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Veterinario_MVVM.Models;
using Veterinario_MVVM.Services;
using Veterinario_MVVM.ViewModels.Base;

namespace Veterinario_MVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MascotasService _mascotasService;
        private readonly Usuario _usuarioActual;

        public string NombreUsuario => _usuarioActual.NombreCompleto;
        public string RolUsuario => _usuarioActual.Rol;

        private ObservableCollection<Mascota> _mascotas = new();
        public ObservableCollection<Mascota> Mascotas
        {
            get => _mascotas;
            set => SetProperty(ref _mascotas, value);
        }

        private Mascota? _mascotaSeleccionada;
        public Mascota? MascotaSeleccionada
        {
            get => _mascotaSeleccionada;
            set
            {
                if (SetProperty(ref _mascotaSeleccionada, value) && value != null)
                {
                    CargarMascotaEnFormulario(value);
                }
            }
        }

        // Propiedades observables del formulario lateral
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private string _nombre = string.Empty;
        public string Nombre { get => _nombre; set => SetProperty(ref _nombre, value); }

        private string _especie = "Perro";
        public string Especie { get => _especie; set => SetProperty(ref _especie, value); }

        private string _raza = string.Empty;
        public string Raza { get => _raza; set => SetProperty(ref _raza, value); }

        private int _edad;
        public int Edad { get => _edad; set => SetProperty(ref _edad, value); }

        private double _peso;
        public double Peso { get => _peso; set => SetProperty(ref _peso, value); }

        private string _propietario = string.Empty;
        public string Propietario { get => _propietario; set => SetProperty(ref _propietario, value); }

        private string _telefono = string.Empty;
        public string Telefono { get => _telefono; set => SetProperty(ref _telefono, value); }

        private string _motivoConsulta = string.Empty;
        public string MotivoConsulta { get => _motivoConsulta; set => SetProperty(ref _motivoConsulta, value); }

        private string _mensajeEstado = "Listo";
        public string MensajeEstado { get => _mensajeEstado; set => SetProperty(ref _mensajeEstado, value); }

        public int TotalPacientes => Mascotas.Count;

        // Comandos
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand LimpiarCommand { get; }
        public ICommand CerrarSesionCommand { get; }

        public MainViewModel() : this(new Usuario { NombreCompleto = "Dr. Antonio García", Rol = "Veterinario Colegiado" }) { }

        public MainViewModel(Usuario usuario)
        {
            _usuarioActual = usuario;
            _mascotasService = new MascotasService();

            Mascotas = new ObservableCollection<Mascota>(_mascotasService.ObtenerTodos());

            GuardarCommand = new RelayCommand(_ => PerformGuardar(), _ => CanExecuteGuardar());
            EliminarCommand = new RelayCommand(_ => PerformEliminar(), _ => MascotaSeleccionada != null);
            LimpiarCommand = new RelayCommand(_ => PerformLimpiar());
            CerrarSesionCommand = new RelayCommand(_ => PerformCerrarSesion());
        }

        private bool CanExecuteGuardar()
        {
            return !string.IsNullOrWhiteSpace(Nombre) && !string.IsNullOrWhiteSpace(Propietario);
        }

        private void PerformGuardar()
        {
            var mascota = new Mascota
            {
                Id = Id,
                Nombre = Nombre.Trim(),
                Especie = Especie,
                Raza = Raza.Trim(),
                Edad = Edad,
                Peso = Peso,
                Propietario = Propietario.Trim(),
                Telefono = Telefono.Trim(),
                MotivoConsulta = MotivoConsulta.Trim()
            };

            _mascotasService.Guardar(mascota);
            Mascotas = new ObservableCollection<Mascota>(_mascotasService.ObtenerTodos());

            MensajeEstado = $"Paciente '{mascota.Nombre}' guardado correctamente.";
            OnPropertyChanged(nameof(TotalPacientes));
            PerformLimpiar();
        }

        private void PerformEliminar()
        {
            if (MascotaSeleccionada != null)
            {
                var result = MessageBox.Show($"¿Está seguro de dar de baja al paciente '{MascotaSeleccionada.Nombre}'?",
                                             "Confirmación de baja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _mascotasService.Eliminar(MascotaSeleccionada.Id);
                    Mascotas = new ObservableCollection<Mascota>(_mascotasService.ObtenerTodos());
                    MensajeEstado = "Paciente eliminado.";
                    OnPropertyChanged(nameof(TotalPacientes));
                    PerformLimpiar();
                }
            }
        }

        private void CargarMascotaEnFormulario(Mascota m)
        {
            Id = m.Id;
            Nombre = m.Nombre;
            Especie = m.Especie;
            Raza = m.Raza;
            Edad = m.Edad;
            Peso = m.Peso;
            Propietario = m.Propietario;
            Telefono = m.Telefono;
            MotivoConsulta = m.MotivoConsulta;
        }

        private void PerformLimpiar()
        {
            MascotaSeleccionada = null;
            Id = 0;
            Nombre = string.Empty;
            Especie = "Perro";
            Raza = string.Empty;
            Edad = 0;
            Peso = 0;
            Propietario = string.Empty;
            Telefono = string.Empty;
            MotivoConsulta = string.Empty;
        }

        private void PerformCerrarSesion()
        {
            var loginView = new Views.LoginView();
            loginView.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Views.MainView)?.Close();
        }
    }
}