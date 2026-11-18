using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Veterinario_MVVM.Services
{
    /// <summary>
    /// Servicio que simula la consulta telemática al Registro Español de Identificación de Animales de Compañía (REIAC).
    /// Permite ilustrar la validación asíncrona desacoplada en MVVM.
    /// </summary>
    public class ReiacService
    {
        // Códigos de microchips simulados que ya constan en la base de datos oficial
        private static readonly HashSet<string> _microchipsRegistrados = new(StringComparer.OrdinalIgnoreCase)
        {
            "999000000000001",
            "999000000000002",
            "111222333444555",
            "123456789012345"
        };

        /// <summary>
        /// Comprueba de forma asíncrona si un microchip ya está registrado a nombre de otro titular.
        /// </summary>
        public async Task<bool> ExisteMicrochipAsync(string microchip)
        {
            if (string.IsNullOrWhiteSpace(microchip) || microchip.Length != 15)
                return false;

            // Simula latencia de consulta de red / web service gubernamental
            await Task.Delay(700);

            return _microchipsRegistrados.Contains(microchip.Trim());
        }
    }
}
