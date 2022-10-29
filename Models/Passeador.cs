using System;
using System.ComponentModel.DataAnnotations;

namespace DogWalkr.Models
{
    public class Passeador
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string Email { get; set; }

        public string Telefone { get; set; }

        public DateTime? Nascimento { get; set; }

        [Required]
        public string Endereco { get; set; }

        [Required]
        public string Cep { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        public string Estado { get; set; }

        public string Descricao { get; set; }

        public string Foto { get; set; }

        public string Horario { get; set; }

        public string Qualificacoes { get; set; }

        [Required]
        public double DistanciaMaxima { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}