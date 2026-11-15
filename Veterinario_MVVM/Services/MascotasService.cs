using System;
using System.Collections.Generic;
using System.Linq;
using Veterinario_MVVM.Models;

namespace Veterinario_MVVM.Services
{
    public class MascotasService
    {
        private readonly List<Mascota> _mascotas;
        private int _nextId = 1;

        public MascotasService()
        {
            _mascotas = new List<Mascota>
            {
                new Mascota
                {
                    Id = _nextId++,
                    Nombre = "Toby",
                    Especie = "Perro",
                    Raza = "Beagle",
                    Edad = 4,
                    Peso = 12.5,
                    Propietario = "Ana Ramos",
                    Telefono = "600112233",
                    MotivoConsulta = "Vacunación anual",
                    FechaRegistro = DateTime.Now.AddDays(-10)
                },
                new Mascota
                {
                    Id = _nextId++,
                    Nombre = "Misi",
                    Especie = "Gato",
                    Raza = "Siamés",
                    Edad = 2,
                    Peso = 3.8,
                    Propietario = "Carlos Gil",
                    Telefono = "655443322",
                    MotivoConsulta = "Revisión bucal",
                    FechaRegistro = DateTime.Now.AddDays(-5)
                },
                new Mascota
                {
                    Id = _nextId++,
                    Nombre = "Piolín",
                    Especie = "Ave",
                    Raza = "Canario",
                    Edad = 1,
                    Peso = 0.05,
                    Propietario = "Elena Mora",
                    Telefono = "699887766",
                    MotivoConsulta = "Corte de uñas",
                    FechaRegistro = DateTime.Now.AddDays(-2)
                },
                new Mascota
                {
                    Id = _nextId++,
                    Nombre = "Rocky",
                    Especie = "Perro",
                    Raza = "Boxer",
                    Edad = 5,
                    Peso = 28.0,
                    Propietario = "Lucas Soto",
                    Telefono = "611223344",
                    MotivoConsulta = "Chequeo general",
                    FechaRegistro = DateTime.Now.AddDays(-1)
                }
            };
        }

        public List<Mascota> ObtenerTodos()
        {
            return new List<Mascota>(_mascotas);
        }

        public Mascota Guardar(Mascota m)
        {
            if (m.Id == 0)
            {
                m.Id = _nextId++;
                m.FechaRegistro = DateTime.Now;
                _mascotas.Add(m);
            }
            else
            {
                var existente = _mascotas.FirstOrDefault(p => p.Id == m.Id);
                if (existente != null)
                {
                    existente.Nombre = m.Nombre;
                    existente.Especie = m.Especie;
                    existente.Raza = m.Raza;
                    existente.Edad = m.Edad;
                    existente.Peso = m.Peso;
                    existente.Propietario = m.Propietario;
                    existente.Telefono = m.Telefono;
                    existente.MotivoConsulta = m.MotivoConsulta;
                }
            }
            return m;
        }

        public bool Eliminar(int id)
        {
            var mascota = _mascotas.FirstOrDefault(m => m.Id == id);
            if (mascota != null)
            {
                return _mascotas.Remove(mascota);
            }
            return false;
        }
    }
}