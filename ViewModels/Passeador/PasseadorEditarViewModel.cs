using System;
using System.ComponentModel.DataAnnotations;
using DogWalkr.Utils.Attributes;
using Microsoft.AspNetCore.Http;

namespace DogWalkr.ViewModels.Passeador
{
    public class PasseadorEditarViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Por favor, insira o seu nome.")]
        public string Nome { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string SenhaSalva { get; set; }

        [EmailAddress(ErrorMessage = "Por favor, insira um e-mail válido.")]
        [Required(ErrorMessage = "Por favor, insira um e-mail.")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(\(?\d{2}\)?\s)?(\d{4,5}\-?\d{4})", ErrorMessage = "Por favor, insira um telefone no formato (XX) XXXXX-XXXX.")]
        public string Telefone { get; set; }

        [ValidBirthday]
        public DateTime? Nascimento { get; set; }

        [Required(ErrorMessage = "Por favor, insira sua rua e número.")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Por favor, insira o seu CEP.")]
        [RegularExpression("[0-9]{5}-[0-9]{3}", ErrorMessage = "Por favor, insira um CEP válido.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Por favor, insira a sua cidade.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Por favor, insira o seu estado.")]
        public string Estado { get; set; }

        public string Descricao { get; set; }

        public IFormFile Foto { get; set; }

        public string FotoSalva { get; set; }

        public string Horario { get; set; }

        public string Qualificacoes { get; set; }

        [Required(ErrorMessage = "Por favor, insira a distância máxima que você aceita percorrer.")]
        public double? DistanciaMaxima { get; set; }

        public string WebRootPath { get; set; }
    }
}