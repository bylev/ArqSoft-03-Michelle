using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogoApp.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Formato { get; set; } = string.Empty;
        public string Artista { get; set; } = string.Empty;

    }
}
