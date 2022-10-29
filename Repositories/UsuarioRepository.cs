using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogWalkr.Models;
using DogWalkr.Utils;
using DogWalkr.ViewModels.Passeador;
using DogWalkr.ViewModels.Usuario;
using Geocoding;
using Geocoding.Microsoft;
using MySqlConnector;

namespace DogWalkr.Repositories
{
    public class UsuarioRepository
    {
        public async Task Create(UsuarioEditarViewModel viewModel)
        {
            using MySqlConnection connection = new MySqlConnection();

            connection.Open();

            string query = "INSERT INTO usuarios (id, nome, login, senha, email, telefone, nascimento, endereco, cep, cidade, estado, latitude, longitude) VALUES (@id, @nome, @login, @senha, @email, @telefone, @nascimento, @endereco, @cep, @cidade, @estado, @latitude, @longitude)";

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

            IGeocoder geocoder = new BingMapsGeocoder("AufZqOR4G3jVcBM1fU582y-vaMQpP28qVc-LTw8bse6eAstuNk_o3A7jqflqrexw");

            string endereco = $"{viewModel.Endereco} {viewModel.Cep} {viewModel.Cidade} - {viewModel.Estado} Brasil";

            var enderecos = await geocoder.GeocodeAsync(endereco);

            command.Parameters.AddWithValue("@latitude", enderecos.First().Coordinates.Latitude);
            command.Parameters.AddWithValue("@longitude", enderecos.First().Coordinates.Longitude);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public Usuario Get(Guid usuarioId)
        {
            using MySqlConnection connection = new MySqlConnection();

            connection.Open();

            string query = "SELECT * FROM usuarios WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", usuarioId);

            using MySqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                var usuario = new Usuario();

                while (dataReader.Read())
                {
                    usuario.Id = dataReader.GetGuid("id");
                    usuario.Nome = dataReader.GetString("nome");
                    usuario.Login = dataReader.GetString("login");
                    usuario.Senha = dataReader.GetString("senha");
                    usuario.Email = dataReader.GetString("email");
                    usuario.Telefone = dataReader.SafeGetString("telefone");
                    usuario.Nascimento = Convert.IsDBNull(dataReader["nascimento"]) ? (DateTime?)null : dataReader.GetDateTime("nascimento");
                    usuario.Endereco = dataReader.GetString("endereco");
                    usuario.Cep = dataReader.GetString("cep");
                    usuario.Cidade = dataReader.GetString("cidade");
                    usuario.Estado = dataReader.GetString("estado");
                    usuario.Latitude = dataReader.GetDouble("latitude");
                    usuario.Longitude = dataReader.GetDouble("longitude");
                }

                dataReader.Close();
                connection.Close();

                return usuario;
            }

            dataReader.Close();
            connection.Close();

            return null;
        }

        public Usuario Get(string login, string senha)
        {
            using MySqlConnection connection = new MySqlConnection();

            connection.Open();

            string query = "SELECT * FROM usuarios WHERE login = @login AND senha = @senha";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@senha", senha);

            using MySqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                var usuario = new Usuario();

                while (dataReader.Read())
                {
                    usuario.Id = dataReader.GetGuid("id");
                    usuario.Nome = dataReader.GetString("nome");
                    usuario.Login = dataReader.GetString("login");
                    usuario.Senha = dataReader.GetString("senha");
                    usuario.Email = dataReader.GetString("email");
                    usuario.Telefone = dataReader.SafeGetString("telefone");
                    usuario.Nascimento = Convert.IsDBNull(dataReader["nascimento"]) ? (DateTime?)null : dataReader.GetDateTime("nascimento");
                    usuario.Endereco = dataReader.GetString("endereco");
                    usuario.Cep = dataReader.GetString("cep");
                    usuario.Cidade = dataReader.GetString("cidade");
                    usuario.Estado = dataReader.GetString("estado");
                    usuario.Latitude = dataReader.GetDouble("latitude");
                    usuario.Longitude = dataReader.GetDouble("longitude");
                }

                dataReader.Close();
                connection.Close();

                return usuario;
            }

            dataReader.Close();
            connection.Close();

            return null;
        }

        public async Task Update(UsuarioEditarViewModel viewModel)
        {
            using MySqlConnection connection = new MySqlConnection();

            connection.Open();

            string query = "UPDATE usuarios SET nome = @nome, senha = @senha, email = @email, telefone = @telefone, nascimento = @nascimento, endereco = @endereco, cep = @cep, cidade = @cidade, estado = @estado, latitude = @latitude, longitude = @longitude WHERE id = @id";

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

            IGeocoder geocoder = new BingMapsGeocoder("AufZqOR4G3jVcBM1fU582y-vaMQpP28qVc-LTw8bse6eAstuNk_o3A7jqflqrexw");

            string endereco = $"{viewModel.Endereco} {viewModel.Cep} {viewModel.Cidade} - {viewModel.Estado} Brasil";

            var enderecos = await geocoder.GeocodeAsync(endereco);

            command.Parameters.AddWithValue("@latitude", enderecos.First().Coordinates.Latitude);
            command.Parameters.AddWithValue("@longitude", enderecos.First().Coordinates.Longitude);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void Delete(Guid usuarioId)
        {
            using MySqlConnection connection = new MySqlConnection();

            connection.Open();

            string query = "DELETE FROM usuarios WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", usuarioId);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public string Like(Guid usuarioId, Guid passeadorId)
        {
            using MySqlConnection connection = new MySqlConnection();

            connection.Open();

            string likeQuery = "INSERT IGNORE INTO passeadores_curtidos (usuario_id, passeador_id) VALUES (@usuario_id, @passeador_id)";

            using MySqlCommand command = new MySqlCommand(likeQuery, connection);

            command.Parameters.AddWithValue("@usuario_id", usuarioId);
            command.Parameters.AddWithValue("@passeador_id", passeadorId);

            command.ExecuteNonQuery();

            string matchQuery = "INSERT IGNORE INTO matches (usuario_id, passeador_id) SELECT usuario_id, passeador_id FROM cachorros_curtidos WHERE EXISTS (SELECT null FROM cachorros_curtidos WHERE usuario_id = @usuario_id AND passeador_id = @passeador_id)";

            using MySqlCommand sqlCommand = new MySqlCommand(matchQuery, connection);

            sqlCommand.Parameters.AddWithValue("@usuario_id", usuarioId);
            sqlCommand.Parameters.AddWithValue("@passeador_id", passeadorId);

            sqlCommand.ExecuteNonQuery();

            if (sqlCommand.LastInsertedId > 0)
            {
                connection.Close();

                return "MATCH";
            }

            connection.Close();

            return "LIKE";
        }

        public void Ignore(Guid usuarioId, Guid passeadorId)
        {
            using MySqlConnection connection = new MySqlConnection();

            connection.Open();

            string ignoreQuery = "INSERT IGNORE INTO passeadores_ignorados (usuario_id, passeador_id) VALUES (@usuario_id, @passeador_id)";

            using MySqlCommand command = new MySqlCommand(ignoreQuery, connection);

            command.Parameters.AddWithValue("@usuario_id", usuarioId);
            command.Parameters.AddWithValue("@passeador_id", passeadorId);

            command.ExecuteNonQuery();

            string deleteQuery = "DELETE FROM passeadores_curtidos WHERE usuario_id = @usuario_id AND passeador_id = @passeador_id";

            command.CommandText = deleteQuery;

            command.ExecuteNonQuery();

            connection.Close();
        }

        public List<PasseadorListarMatchesViewModel> GetMatches(Guid usuarioId)
        {
            using MySqlConnection connection = new MySqlConnection();

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

            string passeadoresQuery = "SELECT * FROM passeadores LEFT JOIN matches ON passeadores.id = matches.passeador_id WHERE usuario_id = @id";

            command.CommandText = passeadoresQuery;

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                var passeadores = new List<PasseadorListarMatchesViewModel>();

                while (reader.Read())
                {
                    var passeador = new PasseadorListarMatchesViewModel
                    {
                        Id = reader.GetGuid("id"),
                        Nome = reader.GetString("nome"),
                        Email = reader.GetString("email"),
                        Telefone = reader.SafeGetString("telefone"),
                        Descricao = reader.SafeGetString("descricao"),
                        Foto = reader.SafeGetString("foto"),
                        Horario = reader.SafeGetString("horario"),
                        Qualificacoes = reader.SafeGetString("qualificacoes"),
                        Distancia = Geography.GetDistance(reader.GetDouble("latitude"),
                                                          reader.GetDouble("longitude"),
                                                          latitude, longitude).ToString("0.##") + " km"
                    };

                    passeadores.Add(passeador);
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