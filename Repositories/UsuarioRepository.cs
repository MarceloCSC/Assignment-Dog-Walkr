using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogWalkr.Database;
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
        private readonly DogWalkrDb _database;

        public UsuarioRepository(DogWalkrDb database)
        {
            _database = database;
        }

        public async Task Create(UsuarioEditarViewModel viewModel)
        {
            string query = "INSERT INTO usuarios (id, nome, login, senha, email, telefone, nascimento, endereco, cep, cidade, estado, latitude, longitude) VALUES (@id, @nome, @login, @senha, @email, @telefone, @nascimento, @endereco, @cep, @cidade, @estado, @latitude, @longitude)";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

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

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task<Usuario> Get(Guid usuarioId)
        {
            string query = "SELECT * FROM usuarios WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", usuarioId);

            using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                var usuario = new Usuario();

                while (await dataReader.ReadAsync())
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

                await dataReader.CloseAsync();
                await _database.Connection.CloseAsync();

                return usuario;
            }

            await dataReader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }

        public async Task<Usuario> Get(string login, string senha)
        {
            string query = "SELECT * FROM usuarios WHERE login = @login AND senha = @senha";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@senha", senha);

            using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                var usuario = new Usuario();

                while (await dataReader.ReadAsync())
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

                await dataReader.CloseAsync();
                await _database.Connection.CloseAsync();

                return usuario;
            }

            await dataReader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }

        public async Task Update(UsuarioEditarViewModel viewModel)
        {
            string query = "UPDATE usuarios SET nome = @nome, senha = @senha, email = @email, telefone = @telefone, nascimento = @nascimento, endereco = @endereco, cep = @cep, cidade = @cidade, estado = @estado, latitude = @latitude, longitude = @longitude WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

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

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task Delete(Guid usuarioId)
        {
            string query = "DELETE FROM usuarios WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", usuarioId);

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task<string> Like(Guid usuarioId, Guid passeadorId)
        {
            string likeQuery = "INSERT IGNORE INTO passeadores_curtidos (usuario_id, passeador_id) VALUES (@usuario_id, @passeador_id)";

            using MySqlCommand command = new MySqlCommand(likeQuery, _database.Connection);

            command.Parameters.AddWithValue("@usuario_id", usuarioId);
            command.Parameters.AddWithValue("@passeador_id", passeadorId);

            await command.ExecuteNonQueryAsync();

            string matchQuery = "INSERT IGNORE INTO matches (usuario_id, passeador_id) SELECT usuario_id, passeador_id FROM cachorros_curtidos WHERE EXISTS (SELECT null FROM cachorros_curtidos WHERE usuario_id = @usuario_id AND passeador_id = @passeador_id)";

            using MySqlCommand sqlCommand = new MySqlCommand(matchQuery, _database.Connection);

            sqlCommand.Parameters.AddWithValue("@usuario_id", usuarioId);
            sqlCommand.Parameters.AddWithValue("@passeador_id", passeadorId);

            sqlCommand.ExecuteNonQuery();

            if (sqlCommand.LastInsertedId > 0)
            {
                await _database.Connection.CloseAsync();

                return "MATCH";
            }

            await _database.Connection.CloseAsync();

            return "LIKE";
        }

        public async Task Ignore(Guid usuarioId, Guid passeadorId)
        {
            string ignoreQuery = "INSERT IGNORE INTO passeadores_ignorados (usuario_id, passeador_id) VALUES (@usuario_id, @passeador_id)";

            using MySqlCommand command = new MySqlCommand(ignoreQuery, _database.Connection);

            command.Parameters.AddWithValue("@usuario_id", usuarioId);
            command.Parameters.AddWithValue("@passeador_id", passeadorId);

            await command.ExecuteNonQueryAsync();

            string deleteQuery = "DELETE FROM passeadores_curtidos WHERE usuario_id = @usuario_id AND passeador_id = @passeador_id";

            command.CommandText = deleteQuery;

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task<List<PasseadorListarMatchesViewModel>> GetMatches(Guid usuarioId)
        {
            double latitude = 0, longitude = 0;

            string query = "SELECT latitude, longitude FROM usuarios WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", usuarioId);

            using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                while (await dataReader.ReadAsync())
                {
                    latitude = dataReader.GetDouble("latitude");
                    longitude = dataReader.GetDouble("longitude");
                }
            }

            await dataReader.CloseAsync();

            string passeadoresQuery = "SELECT * FROM passeadores LEFT JOIN matches ON passeadores.id = matches.passeador_id WHERE usuario_id = @id";

            command.CommandText = passeadoresQuery;

            using MySqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                var passeadores = new List<PasseadorListarMatchesViewModel>();

                while (await reader.ReadAsync())
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

                await reader.CloseAsync();
                await _database.Connection.CloseAsync();

                return passeadores;
            }

            await reader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }
    }
}