using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DogWalkr.Models;
using DogWalkr.Utils;
using DogWalkr.ViewModels.Cachorro;
using DogWalkr.ViewModels.Passeador;
using DogWalkr.ViewModels.Usuario;
using Geocoding;
using Geocoding.Microsoft;
using MySqlConnector;

namespace DogWalkr.Repositories
{
    public class PasseadorRepository
    {
        private const string DadosConexao = "Database=dog_walkr; Data Source=localhost; User Id=root;";

        public async Task Create(PasseadorCadastrarViewModel viewModel)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            string query = "INSERT INTO passeadores (id, nome, login, senha, email, telefone, nascimento, endereco, cep, cidade, estado, descricao, foto, horario, qualificacoes, distancia_maxima, latitude, longitude) VALUES (@id, @nome, @login, @senha, @email, @telefone, @nascimento, @endereco, @cep, @cidade, @estado, @descricao, @foto, @horario, @qualificacoes, @distancia_maxima, @latitude, @longitude)";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", viewModel.Id);
            command.Parameters.AddWithValue("@nome", viewModel.Nome);
            command.Parameters.AddWithValue("@login", viewModel.Login);
            command.Parameters.AddWithValue("@senha", viewModel.Senha);
            command.Parameters.AddWithValue("@email", viewModel.Email);
            command.Parameters.AddWithValue("@telefone", viewModel.Telefone);
            command.Parameters.AddWithValue("@nascimento", viewModel.Nascimento);
            command.Parameters.AddWithValue("@endereco", viewModel.Endereco);
            command.Parameters.AddWithValue("@cep", viewModel.Cep);
            command.Parameters.AddWithValue("@cidade", viewModel.Cidade);
            command.Parameters.AddWithValue("@estado", viewModel.Estado);
            command.Parameters.AddWithValue("@descricao", viewModel.Descricao);
            command.Parameters.AddWithValue("@horario", viewModel.Horario);
            command.Parameters.AddWithValue("@qualificacoes", viewModel.Qualificacoes);
            command.Parameters.AddWithValue("@distancia_maxima", viewModel.DistanciaMaxima);

            string fileName = string.Empty;

            if (viewModel.Foto != null)
            {
                fileName = Guid.NewGuid().ToString() + "_" + viewModel.Foto.FileName;
                string folder = Path.Combine(viewModel.WebRootPath, "images");
                string filePath = Path.Combine(folder, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);

                viewModel.Foto.CopyTo(fileStream);
            }

            command.Parameters.AddWithValue("@foto", fileName);

            IGeocoder geocoder = new BingMapsGeocoder("AufZqOR4G3jVcBM1fU582y-vaMQpP28qVc-LTw8bse6eAstuNk_o3A7jqflqrexw");

            string endereco = $"{viewModel.Endereco} {viewModel.Cep} {viewModel.Cidade} - {viewModel.Estado} Brasil";

            var enderecos = await geocoder.GeocodeAsync(endereco);

            command.Parameters.AddWithValue("@latitude", enderecos.First().Coordinates.Latitude);
            command.Parameters.AddWithValue("@longitude", enderecos.First().Coordinates.Longitude);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public Passeador Get(Guid id)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            string query = "SELECT * FROM passeadores WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            using MySqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                var passeador = new Passeador();

                while (dataReader.Read())
                {
                    passeador.Id = dataReader.GetGuid("id");
                    passeador.Nome = dataReader.GetString("nome");
                    passeador.Login = dataReader.GetString("login");
                    passeador.Senha = dataReader.GetString("senha");
                    passeador.Email = dataReader.GetString("email");
                    passeador.Telefone = dataReader.SafeGetString("telefone");
                    passeador.Nascimento = Convert.IsDBNull(dataReader["nascimento"]) ? (DateTime?)null : dataReader.GetDateTime("nascimento");
                    passeador.Endereco = dataReader.GetString("endereco");
                    passeador.Cep = dataReader.GetString("cep");
                    passeador.Cidade = dataReader.GetString("cidade");
                    passeador.Estado = dataReader.GetString("estado");
                    passeador.Descricao = dataReader.SafeGetString("descricao");
                    passeador.Foto = dataReader.SafeGetString("foto");
                    passeador.Horario = dataReader.SafeGetString("horario");
                    passeador.Qualificacoes = dataReader.SafeGetString("qualificacoes");
                    passeador.DistanciaMaxima = dataReader.GetDouble("distancia_maxima");
                    passeador.Latitude = dataReader.GetDouble("latitude");
                    passeador.Longitude = dataReader.GetDouble("longitude");
                }

                dataReader.Close();
                connection.Close();

                return passeador;
            }

            dataReader.Close();
            connection.Close();

            return null;
        }

        public Passeador Get(string login, string senha)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            string query = "SELECT * FROM passeadores WHERE login = @login AND senha = @senha";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@senha", senha);

            using MySqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                var passeador = new Passeador();

                while (dataReader.Read())
                {
                    passeador.Id = dataReader.GetGuid("id");
                    passeador.Nome = dataReader.GetString("nome");
                    passeador.Login = dataReader.GetString("login");
                    passeador.Senha = dataReader.GetString("senha");
                    passeador.Email = dataReader.GetString("email");
                    passeador.Telefone = dataReader.SafeGetString("telefone");
                    passeador.Nascimento = Convert.IsDBNull(dataReader["nascimento"]) ? (DateTime?)null : dataReader.GetDateTime("nascimento");
                    passeador.Endereco = dataReader.GetString("endereco");
                    passeador.Cep = dataReader.GetString("cep");
                    passeador.Cidade = dataReader.GetString("cidade");
                    passeador.Estado = dataReader.GetString("estado");
                    passeador.Descricao = dataReader.SafeGetString("descricao");
                    passeador.Foto = dataReader.SafeGetString("foto");
                    passeador.Horario = dataReader.SafeGetString("horario");
                    passeador.Qualificacoes = dataReader.SafeGetString("qualificacoes");
                    passeador.DistanciaMaxima = dataReader.GetDouble("distancia_maxima");
                    passeador.Latitude = dataReader.GetDouble("latitude");
                    passeador.Longitude = dataReader.GetDouble("longitude");
                }

                dataReader.Close();
                connection.Close();

                return passeador;
            }

            dataReader.Close();
            connection.Close();

            return null;
        }

        public async Task Update(PasseadorEditarViewModel viewModel)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            string query = "UPDATE passeadores SET nome = @nome, senha = @senha, email = @email, telefone = @telefone, nascimento = @nascimento, endereco = @endereco, cep = @cep, cidade = @cidade, estado = @estado, descricao = @descricao, foto = @foto, horario = @horario, qualificacoes = @qualificacoes, distancia_maxima = @distancia_maxima, latitude = @latitude, longitude = @longitude WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", viewModel.Id);
            command.Parameters.AddWithValue("@nome", viewModel.Nome);
            command.Parameters.AddWithValue("@senha", String.IsNullOrEmpty(viewModel.Senha) ? viewModel.SenhaSalva : viewModel.Senha);
            command.Parameters.AddWithValue("@email", viewModel.Email);
            command.Parameters.AddWithValue("@telefone", viewModel.Telefone);
            command.Parameters.AddWithValue("@nascimento", viewModel.Nascimento);
            command.Parameters.AddWithValue("@endereco", viewModel.Endereco);
            command.Parameters.AddWithValue("@cep", viewModel.Cep);
            command.Parameters.AddWithValue("@cidade", viewModel.Cidade);
            command.Parameters.AddWithValue("@estado", viewModel.Estado);
            command.Parameters.AddWithValue("@descricao", viewModel.Descricao);
            command.Parameters.AddWithValue("@horario", viewModel.Horario);
            command.Parameters.AddWithValue("@qualificacoes", viewModel.Qualificacoes);
            command.Parameters.AddWithValue("@distancia_maxima", viewModel.DistanciaMaxima);

            string fileName = viewModel.FotoSalva;

            if (viewModel.Foto != null)
            {
                fileName = Guid.NewGuid().ToString() + "_" + viewModel.Foto.FileName;
                string folder = Path.Combine(viewModel.WebRootPath, "images");
                string filePath = Path.Combine(folder, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);

                viewModel.Foto.CopyTo(fileStream);
            }

            command.Parameters.AddWithValue("@foto", fileName);

            IGeocoder geocoder = new BingMapsGeocoder("AufZqOR4G3jVcBM1fU582y-vaMQpP28qVc-LTw8bse6eAstuNk_o3A7jqflqrexw");

            string endereco = $"{viewModel.Endereco} {viewModel.Cep} {viewModel.Cidade} - {viewModel.Estado} Brasil";

            var enderecos = await geocoder.GeocodeAsync(endereco);

            command.Parameters.AddWithValue("@latitude", enderecos.First().Coordinates.Latitude);
            command.Parameters.AddWithValue("@longitude", enderecos.First().Coordinates.Longitude);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void Delete(Guid id)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            string query = "DELETE FROM passeadores WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public string Like(Guid passeadorId, Guid cachorroId, Guid usuarioId)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            string likeQuery = "INSERT IGNORE INTO cachorros_curtidos (passeador_id, cachorro_id, usuario_id) VALUES (@passeador_id, @cachorro_id, @usuario_id)";

            using MySqlCommand command = new MySqlCommand(likeQuery, connection);

            command.Parameters.AddWithValue("@passeador_id", passeadorId);
            command.Parameters.AddWithValue("@cachorro_id", cachorroId);
            command.Parameters.AddWithValue("@usuario_id", usuarioId);

            command.ExecuteNonQuery();

            string matchQuery = "INSERT IGNORE INTO matches (usuario_id, passeador_id) SELECT usuario_id, passeador_id FROM passeadores_curtidos WHERE EXISTS (SELECT null FROM passeadores_curtidos WHERE usuario_id = @usuario_id AND passeador_id = @passeador_id)";

            using MySqlCommand sqlCommand = new MySqlCommand(matchQuery, connection);

            sqlCommand.Parameters.AddWithValue("@passeador_id", passeadorId);
            sqlCommand.Parameters.AddWithValue("@usuario_id", usuarioId);

            sqlCommand.ExecuteNonQuery();

            if (sqlCommand.LastInsertedId > 0)
            {
                connection.Close();

                return "MATCH";
            }

            connection.Close();

            return "LIKE";
        }

        public void Ignore(Guid passeadorId, Guid cachorroId, Guid usuarioId)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            string ignoreQuery = "INSERT IGNORE INTO cachorros_ignorados (passeador_id, cachorro_id, usuario_id) VALUES (@passeador_id, @cachorro_id, @usuario_id)";

            using MySqlCommand command = new MySqlCommand(ignoreQuery, connection);

            command.Parameters.AddWithValue("@passeador_id", passeadorId);
            command.Parameters.AddWithValue("@cachorro_id", cachorroId);
            command.Parameters.AddWithValue("@usuario_id", usuarioId);

            command.ExecuteNonQuery();

            string deleteQuery = "DELETE FROM cachorros_curtidos WHERE passeador_id = @passeador_id AND cachorro_id = @cachorro_id";

            command.CommandText = deleteQuery;

            command.ExecuteNonQuery();

            connection.Close();
        }

        public List<UsuarioListarMatchesViewModel> GetMatches(Guid id)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            double latitude = 0, longitude = 0;

            string query = "SELECT latitude, longitude FROM passeadores WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            using MySqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    latitude = dataReader.GetDouble("latitude");
                    longitude = dataReader.GetDouble("longitude");
                }
            }

            dataReader.Close();

            string usuariosQuery = "SELECT usuarios.id as usuario_id, usuarios.*, cachorros.id as cachorro_id, cachorros.nome as cachorro_nome, cachorros.* FROM usuarios LEFT JOIN matches ON usuarios.id = matches.usuario_id LEFT JOIN cachorros ON cachorros.usuario_id = usuarios.id WHERE passeador_id = @id";

            command.CommandText = usuariosQuery;

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                var usuariosDto = new List<UsuarioCachorroDto>();

                while (reader.Read())
                {
                    var usuario = new UsuarioCachorroDto
                    {
                        Id = reader.GetGuid("id"),
                        Nome = reader.GetString("nome"),
                        Email = reader.GetString("email"),
                        Telefone = reader.SafeGetString("telefone"),
                        Distancia = Geography.GetDistance(reader.GetDouble("latitude"),
                                                          reader.GetDouble("longitude"),
                                                          latitude, longitude).ToString("0.##") + " km",
                        CachorroId = reader.GetGuid("cachorro_id"),
                        CachorroNome = reader.GetString("cachorro_nome"),
                        Raca = reader.SafeGetString("raca"),
                        Porte = reader.SafeGetString("porte"),
                        Idade = reader.SafeGetString("idade"),
                        Sexo = reader.SafeGetString("sexo"),
                        Descricao = reader.SafeGetString("descricao"),
                        Foto = reader.SafeGetString("foto")
                    };

                    usuariosDto.Add(usuario);
                }

                reader.Close();
                connection.Close();

                var usuarios = usuariosDto.GroupBy(usuario => usuario.Id).Select(usuarioGrupo =>
                {
                    var usuario = usuarioGrupo.First();

                    return new UsuarioListarMatchesViewModel
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        Telefone = usuario.Telefone,
                        Distancia = usuario.Distancia,
                        Cachorros = usuarioGrupo.Select(cachorro => new CachorroListarMatchesViewModel
                        {
                            Id = cachorro.CachorroId,
                            Nome = cachorro.CachorroNome,
                            Raca = cachorro.Raca,
                            Porte = cachorro.Porte,
                            Idade = cachorro.Idade,
                            Sexo = cachorro.Sexo,
                            Descricao = cachorro.Descricao,
                            Foto = cachorro.Foto
                        })
                    };
                }).ToList();

                return usuarios;
            }

            reader.Close();
            connection.Close();

            return null;
        }

        public List<PasseadorProcurarViewModel> Search(Guid usuarioId)
        {
            using MySqlConnection connection = new MySqlConnection(DadosConexao);

            connection.Open();

            double latitude = 0, longitude = 0;

            string query = "SELECT latitude, longitude FROM usuarios WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", usuarioId);

            using MySqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    latitude = dataReader.GetDouble("latitude");
                    longitude = dataReader.GetDouble("longitude");
                }
            }

            dataReader.Close();

            string searchQuery = "SELECT * FROM passeadores WHERE NOT EXISTS (SELECT null FROM passeadores_curtidos WHERE passeadores.id = passeadores_curtidos.passeador_id AND usuario_id = @id) AND NOT EXISTS (SELECT null FROM passeadores_ignorados WHERE passeadores.id = passeadores_ignorados.passeador_id AND usuario_id = @id) AND NOT EXISTS (SELECT null FROM matches WHERE passeadores.id = matches.passeador_id AND usuario_id = @id)";

            command.CommandText = searchQuery;

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                var passeadores = new List<PasseadorProcurarViewModel>();

                while (reader.Read())
                {
                    double distance = Geography.GetDistance(reader.GetDouble("latitude"),
                                                            reader.GetDouble("longitude"),
                                                            latitude, longitude);

                    if (distance <= reader.GetDouble("distancia_maxima"))
                    {
                        var passeador = new PasseadorProcurarViewModel
                        {
                            Id = reader.GetGuid("id"),
                            Nome = reader.GetString("nome"),
                            Descricao = reader.SafeGetString("descricao"),
                            Foto = reader.SafeGetString("foto"),
                            Horario = reader.SafeGetString("horario"),
                            Qualificacoes = reader.SafeGetString("qualificacoes"),
                            Distancia = distance.ToString("0.##") + " km"
                        };

                        passeadores.Add(passeador);
                    }
                }

                reader.Close();
                connection.Close();

                return passeadores;
            }

            reader.Close();
            connection.Close();

            return null;
        }
    }
}