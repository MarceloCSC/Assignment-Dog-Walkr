using System;
using System.ComponentModel.DataAnnotations;
using DogWalkr.Utils.Attributes;
using Microsoft.AspNetCore.Http;

namespace DogWalkr.ViewModels.Passeador
{
    public class PasseadorCadastrarViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Por favor, insira o seu nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, insira o login.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Por favor, insira uma senha.")]
        public string Senha { get; set; }

        [EmailAddress(ErrorMessage = "Por favor, insira um e-mail válido.")]
        [Required(ErrorMessage = "Por favor, insira um e-mail.")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(\(?\d{2}\)?\s)?(\d{4,5}\-?\d{4})", ErrorMessage = "Por favor, insira um telefone válido.")]
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

        public string Horario { get; set; }

        public string Qualificacoes { get; set; }

        [Required(ErrorMessage = "Insira a distância máxima que aceita percorrer.")]
        public double? DistanciaMaxima { get; set; }

        public string WebRootPath { get; set; }
    }
}