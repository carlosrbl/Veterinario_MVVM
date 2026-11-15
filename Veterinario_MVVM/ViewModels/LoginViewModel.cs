using System.Linq;
using System.Windows;
using System.Windows.Input;
using Veterinario_MVVM.Services;
using Veterinario_MVVM.ViewModels.Base;
using Veterinario_MVVM.Views;

namespace Veterinario_MVVM.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _mensajeError = string.Empty;
        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        private bool _hayError;
        public bool HayError
        {
            get => _hayError;
            set => SetProperty(ref _hayError, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand LimpiarCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand(_ => PerformLogin(), _ => CanExecuteLogin());
            LimpiarCommand = new RelayCommand(_ => PerformLimpiar());
        }

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsBusy;
        }

        private void PerformLogin()
        {
            IsBusy = true;
            HayError = false;
            MensajeError = string.Empty;

            try
            {
                var usuario = _authService.ValidarLogin(Username, Password);
                if (usuario != null)
                {
                    var mainView = new MainView();
                    mainView.DataContext = new MainViewModel(usuario);
                    mainView.Show();

                    Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginView)?.Close();
                }
                else
                {
                    MensajeError = "Usuario o clave incorrectos.";
                    HayError = true;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void PerformLimpiar()
        {
            Username = string.Empty;
            Password = string.Empty;
            MensajeError = string.Empty;
            HayError = false;
        }
    }
}