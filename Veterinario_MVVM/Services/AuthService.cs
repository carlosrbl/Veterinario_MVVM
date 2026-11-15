using System;
using System.Collections.Generic;
using System.Linq;
using Veterinario_MVVM.Models;

namespace Veterinario_MVVM.Services
{
    public class AuthService
    {
        private readonly List<Usuario> _usuarios;

        public AuthService()
        {
            _usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Username = "admin",
                    Password = "123",
                    NombreCompleto = "Administrador del Sistema",
                    Rol = "Administrador Clínico"
                },
                new Usuario
                {
                    Username = "veterinario",
                    Password = "123",
                    NombreCompleto = "Dr. Antonio García",
                    Rol = "Veterinario Colegiado"
                }
            };
        }

        public Usuario? ValidarLogin(string username, string password)
        {
            return _usuarios.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }
    }
}