using System.ComponentModel.DataAnnotations;

namespace DogWalkr.ViewModels.Shared
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Por favor, insira o login.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Por favor, insira uma senha.")]
        public string Senha { get; set; }
    }
}