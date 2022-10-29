using System;
using System.ComponentModel.DataAnnotations;
using DogWalkr.Utils.Attributes;

namespace DogWalkr.Models
{
    public class Usuario
    {
        [Required]
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

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public Guid PasseadorContratado { get; set; }
    }
}