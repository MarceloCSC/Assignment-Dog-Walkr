using System;
using System.ComponentModel.DataAnnotations;

namespace DogWalkr.Models
{
    public class Cachorro
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Raca { get; set; }

        public string Porte { get; set; }

        public string Idade { get; set; }

        public string Sexo { get; set; }

        public string Descricao { get; set; }

        public string Foto { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}