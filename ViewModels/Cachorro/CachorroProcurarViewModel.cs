using System;

namespace DogWalkr.ViewModels.Cachorro
{
    public class CachorroProcurarViewModel
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public string Nome { get; set; }

        public string Raca { get; set; }

        public string Porte { get; set; }

        public string Idade { get; set; }

        public string Sexo { get; set; }

        public string Descricao { get; set; }

        public string Foto { get; set; }

        public string Distancia { get; set; }
    }
}