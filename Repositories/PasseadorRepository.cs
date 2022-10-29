using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DogWalkr.Database;
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
        private readonly DogWalkrDb _database;

        public PasseadorRepository(DogWalkrDb database)
        {
            _database = database;
        }

        public async Task Create(PasseadorCadastrarViewModel viewModel)
        {
            string query = "INSERT INTO passeadores (id, nome, login, senha, email, telefone, nascimento, endereco, cep, cidade, estado, descricao, foto, horario, qualificacoes, distancia_maxima, latitude, longitude) VALUES (@id, @nome, @login, @senha, @email, @telefone, @nascimento, @endereco, @cep, @cidade, @estado, @descricao, @foto, @horario, @qualificacoes, @distancia_maxima, @latitude, @longitude)";

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

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task<Passeador> Get(Guid id)
        {
            string query = "SELECT * FROM passeadores WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", id);

            using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                var passeador = new Passeador();

                while (await dataReader.ReadAsync())
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

                await dataReader.CloseAsync();
                await _database.Connection.CloseAsync();

                return passeador;
            }

            await dataReader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }

        public async Task<Passeador> Get(string login, string senha)
        {
            string query = "SELECT * FROM passeadores WHERE login = @login AND senha = @senha";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@senha", senha);

            using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                var passeador = new Passeador();

                while (await dataReader.ReadAsync())
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

                await dataReader.CloseAsync();
                await _database.Connection.CloseAsync();

                return passeador;
            }

            await dataReader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }

        public async Task Update(PasseadorEditarViewModel viewModel)
        {
            string query = "UPDATE passeadores SET nome = @nome, senha = @senha, email = @email, telefone = @telefone, nascimento = @nascimento, endereco = @endereco, cep = @cep, cidade = @cidade, estado = @estado, descricao = @descricao, foto = @foto, horario = @horario, qualificacoes = @qualificacoes, distancia_maxima = @distancia_maxima, latitude = @latitude, longitude = @longitude WHERE id = @id";

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

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task Delete(Guid id)
        {
            string query = "DELETE FROM passeadores WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task<string> Like(Guid passeadorId, Guid cachorroId, Guid usuarioId)
        {
            string likeQuery = "INSERT IGNORE INTO cachorros_curtidos (passeador_id, cachorro_id, usuario_id) VALUES (@passeador_id, @cachorro_id, @usuario_id)";

            using MySqlCommand command = new MySqlCommand(likeQuery, _database.Connection);

            command.Parameters.AddWithValue("@passeador_id", passeadorId);
            command.Parameters.AddWithValue("@cachorro_id", cachorroId);
            command.Parameters.AddWithValue("@usuario_id", usuarioId);

            await command.ExecuteNonQueryAsync();

            string matchQuery = "INSERT IGNORE INTO matches (usuario_id, passeador_id) SELECT usuario_id, passeador_id FROM passeadores_curtidos WHERE EXISTS (SELECT null FROM passeadores_curtidos WHERE usuario_id = @usuario_id AND passeador_id = @passeador_id)";

            using MySqlCommand sqlCommand = new MySqlCommand(matchQuery, _database.Connection);

            sqlCommand.Parameters.AddWithValue("@passeador_id", passeadorId);
            sqlCommand.Parameters.AddWithValue("@usuario_id", usuarioId);

            await sqlCommand.ExecuteNonQueryAsync();

            if (sqlCommand.LastInsertedId > 0)
            {
                await _database.Connection.CloseAsync();

                return "MATCH";
            }

            await _database.Connection.CloseAsync();

            return "LIKE";
        }

        public async Task Ignore(Guid passeadorId, Guid cachorroId, Guid usuarioId)
        {
            string ignoreQuery = "INSERT IGNORE INTO cachorros_ignorados (passeador_id, cachorro_id, usuario_id) VALUES (@passeador_id, @cachorro_id, @usuario_id)";

            using MySqlCommand command = new MySqlCommand(ignoreQuery, _database.Connection);

            command.Parameters.AddWithValue("@passeador_id", passeadorId);
            command.Parameters.AddWithValue("@cachorro_id", cachorroId);
            command.Parameters.AddWithValue("@usuario_id", usuarioId);

            await command.ExecuteNonQueryAsync();

            string deleteQuery = "DELETE FROM cachorros_curtidos WHERE passeador_id = @passeador_id AND cachorro_id = @cachorro_id";

            command.CommandText = deleteQuery;

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task<List<UsuarioListarMatchesViewModel>> GetMatches(Guid id)
        {
            double latitude = 0, longitude = 0;

            string query = "SELECT latitude, longitude FROM passeadores WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", id);

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

            string usuariosQuery = "SELECT usuarios.id as usuario_id, usuarios.*, cachorros.id as cachorro_id, cachorros.nome as cachorro_nome, cachorros.* FROM usuarios LEFT JOIN matches ON usuarios.id = matches.usuario_id LEFT JOIN cachorros ON cachorros.usuario_id = usuarios.id WHERE passeador_id = @id";

            command.CommandText = usuariosQuery;

            using MySqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                var usuariosDto = new List<UsuarioCachorroDto>();

                while (await reader.ReadAsync())
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

                await reader.CloseAsync();
                await _database.Connection.CloseAsync();

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

            await reader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }

        public async Task<List<PasseadorProcurarViewModel>> Search(Guid usuarioId)
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

            string searchQuery = "SELECT * FROM passeadores WHERE NOT EXISTS (SELECT null FROM passeadores_curtidos WHERE passeadores.id = passeadores_curtidos.passeador_id AND usuario_id = @id) AND NOT EXISTS (SELECT null FROM passeadores_ignorados WHERE passeadores.id = passeadores_ignorados.passeador_id AND usuario_id = @id) AND NOT EXISTS (SELECT null FROM matches WHERE passeadores.id = matches.passeador_id AND usuario_id = @id)";

            command.CommandText = searchQuery;

            using MySqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                var passeadores = new List<PasseadorProcurarViewModel>();

                while (await reader.ReadAsync())
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