using System;

namespace DogWalkr.Repositories
{
    public class UsuarioCachorroDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string Distancia { get; set; }

        public Guid CachorroId { get; set; }

        public string CachorroNome { get; set; }

        public string Raca { get; set; }

        public string Porte { get; set; }

        public string Idade { get; set; }

        public string Sexo { get; set; }

        public string Descricao { get; set; }

        public string Foto { get; set; }
    }
}