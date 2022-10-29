using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DogWalkr.ViewModels.Cachorro
{
    public class CachorroCadastrarViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o nome do seu cachorro.")]
        public string Nome { get; set; }

        public string Raca { get; set; }

        public string Porte { get; set; }

        public string Idade { get; set; }

        public string Sexo { get; set; }

        public string Descricao { get; set; }

        public IFormFile Foto { get; set; }

        public string WebRootPath { get; set; }

    }
}