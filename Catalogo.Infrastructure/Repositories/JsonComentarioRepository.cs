using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Catalogo.Domain.Interfaces;
using Catalogo.Domain.Models;

namespace Catalogo.Infrastructure.Repositories
{
    public class JsonComentarioRepository : IComentarioRepository
    {
        private readonly string _filePath;

        public JsonComentarioRepository(string filePath)
        {
            _filePath = filePath;

            // Si la carpeta no existe, crearla
            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);

            // Si el archivo no existe, crearlo con una lista vacía
            if (!File.Exists(_filePath))
            {
                var comentariosVacios = new List<Comentario>();
                GuardarComentarios(comentariosVacios);
            }
        }

        /// <summary>
        /// Obtiene todos los comentarios de un ítem específico
        /// </summary>
        public List<Comentario> ObtenerPorItemId(int itemId)
        {
            var comentarios = ObtenerTodos();
            return comentarios.Where(c => c.ItemId == itemId).ToList();
        }

        /// <summary>
        /// Obtiene todos los comentarios
        /// </summary>
        public List<Comentario> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<Comentario>();

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Comentario>>(json) 
                    ?? new List<Comentario>();
            }
            catch (Exception)
            {
                return new List<Comentario>();
            }
        }

        /// <summary>
        /// Obtiene un comentario por su ID
        /// </summary>
        public Comentario? ObtenerPorId(int id)
        {
            var comentarios = ObtenerTodos();
            return comentarios.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Agrega un nuevo comentario
        /// </summary>
        public bool Agregar(Comentario comentario)
        {
            try
            {
                var comentarios = ObtenerTodos();

                // Generar nuevo ID (el mayor ID + 1)
                int nuevoId = comentarios.Any() ? comentarios.Max(c => c.Id) + 1 : 1;
                comentario.Id = nuevoId;

                comentarios.Add(comentario);
                GuardarComentarios(comentarios);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Elimina un comentario por su ID
        /// </summary>
        public bool Eliminar(int id)
        {
            try
            {
                var comentarios = ObtenerTodos();
                var comentario = comentarios.FirstOrDefault(c => c.Id == id);

                if (comentario == null)
                    return false;

                comentarios.Remove(comentario);
                GuardarComentarios(comentarios);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene el promedio de rating de un ítem
        /// </summary>
        public double ObtenerPromedioRating(int itemId)
        {
            var comentarios = ObtenerPorItemId(itemId);

            if (comentarios.Count == 0)
                return 0;

            return comentarios.Average(c => c.Rating);
        }

        /// <summary>
        /// Guarda la lista de comentarios en el archivo JSON
        /// </summary>
        private void GuardarComentarios(List<Comentario> comentarios)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(comentarios, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar comentarios: {ex.Message}", ex);
            }
        }
    }
}
