using System;
using System.Collections.Generic;
using DogWalkr.ViewModels.Cachorro;

namespace DogWalkr.ViewModels.Usuario
{
    public class UsuarioListarMatchesViewModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public IEnumerable<CachorroListarMatchesViewModel> Cachorros { get; set; }

        public string Distancia { get; set; }
    }
}