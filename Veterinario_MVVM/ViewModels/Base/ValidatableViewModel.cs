using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Veterinario_MVVM.ViewModels.Base
{
    /// <summary>
    /// Clase base para ViewModels que requieren validación desacoplada
    /// mediante INotifyDataErrorInfo y DataAnnotations en WPF.
    /// </summary>
    public abstract class ValidatableViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new();

        public bool HasErrors => _errors.Values.Any(list => list.Count > 0);

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return _errors.Values.SelectMany(x => x).ToList();
            }

            return _errors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
        }

        protected void OnErrorsChanged([CallerMemberName] string? propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
            CommandManager.InvalidateRequerySuggested();
        }

        public void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        public void ClearErrors([CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                _errors.Clear();
                OnErrorsChanged(string.Empty);
                return;
            }

            if (_errors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }

        public void SetErrors(string propertyName, IEnumerable<string> errors)
        {
            ClearErrors(propertyName);
            var list = errors.ToList();
            if (list.Count > 0)
            {
                _errors[propertyName] = list;
                OnErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// Valida una propiedad contra sus atributos de DataAnnotations.
        /// </summary>
        protected void ValidateProperty(object? value, [CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) return;

            var context = new ValidationContext(this) { MemberName = propertyName };
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(value, context, results);

            if (results.Count > 0)
            {
                SetErrors(propertyName, results.Select(r => r.ErrorMessage ?? "Error de validación"));
            }
            else
            {
                ClearErrors(propertyName);
            }
        }

        /// <summary>
        /// Valida todas las propiedades públicas del ViewModel decoradas con DataAnnotations.
        /// </summary>
        public bool ValidateAll()
        {
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();

            _errors.Clear();
            Validator.TryValidateObject(this, context, results, validateAllProperties: true);

            foreach (var group in results.GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty))
            {
                if (!string.IsNullOrEmpty(group.Key))
                {
                    _errors[group.Key] = group.Select(r => r.ErrorMessage ?? "Error").ToList();
                    OnErrorsChanged(group.Key);
                }
            }

            OnPropertyChanged(nameof(HasErrors));
            CommandManager.InvalidateRequerySuggested();
            return !HasErrors;
        }
    }
}
