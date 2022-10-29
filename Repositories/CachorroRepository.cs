using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DogWalkr.Database;
using DogWalkr.Models;
using DogWalkr.Utils;
using DogWalkr.ViewModels.Cachorro;
using MySqlConnector;

namespace DogWalkr.Repositories
{
    public class CachorroRepository
    {
        private readonly DogWalkrDb _database;

        internal CachorroRepository(DogWalkrDb database)
        {
            _database = database;
        }

        public async Task Create(CachorroCadastrarViewModel viewModel)
        {
            double latitude = 0, longitude = 0;

            string query = "SELECT latitude, longitude FROM usuarios WHERE id = @usuario_id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@usuario_id", viewModel.UsuarioId);

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

            string insertQuery = "INSERT INTO cachorros (id, usuario_id, nome, raca, porte, idade, sexo, descricao, foto, latitude, longitude) VALUES (@id, @usuario_id, @nome, @raca, @porte, @idade, @sexo, @descricao, @foto, @latitude, @longitude)";

            command.CommandText = insertQuery;
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@id", viewModel.Id);
            command.Parameters.AddWithValue("@usuario_id", viewModel.UsuarioId);
            command.Parameters.AddWithValue("@nome", viewModel.Nome);
            command.Parameters.AddWithValue("@raca", viewModel.Raca);
            command.Parameters.AddWithValue("@porte", viewModel.Porte);
            command.Parameters.AddWithValue("@idade", viewModel.Idade);
            command.Parameters.AddWithValue("@sexo", viewModel.Sexo);
            command.Parameters.AddWithValue("@descricao", viewModel.Descricao);
            command.Parameters.AddWithValue("@latitude", latitude);
            command.Parameters.AddWithValue("@longitude", longitude);

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

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task<Cachorro> Get(Guid id)
        {
            string query = "SELECT * FROM cachorros WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", id);

            using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                var cachorro = new Cachorro();

                while (await dataReader.ReadAsync())
                {
                    cachorro.Id = dataReader.GetGuid("id");
                    cachorro.UsuarioId = dataReader.GetGuid("usuario_id");
                    cachorro.Nome = dataReader.GetString("nome");
                    cachorro.Raca = dataReader.SafeGetString("raca");
                    cachorro.Porte = dataReader.SafeGetString("porte");
                    cachorro.Idade = dataReader.SafeGetString("idade");
                    cachorro.Sexo = dataReader.SafeGetString("sexo");
                    cachorro.Descricao = dataReader.SafeGetString("descricao");
                    cachorro.Foto = dataReader.SafeGetString("foto");
                    cachorro.Latitude = dataReader.GetDouble("latitude");
                    cachorro.Longitude = dataReader.GetDouble("longitude");
                }

                await dataReader.CloseAsync();
                await _database.Connection.CloseAsync();

                return cachorro;
            }

            await dataReader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }

        public async Task<List<Cachorro>> GetAll(Guid usuarioId)
        {
            string query = "SELECT * FROM cachorros WHERE usuario_id = @usuario_id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@usuario_id", usuarioId);

            using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                var cachorros = new List<Cachorro>();

                while (await dataReader.ReadAsync())
                {
                    var cachorro = new Cachorro
                    {
                        Id = dataReader.GetGuid("id"),
                        UsuarioId = dataReader.GetGuid("usuario_id"),
                        Nome = dataReader.GetString("nome"),
                        Raca = dataReader.SafeGetString("raca"),
                        Porte = dataReader.SafeGetString("porte"),
                        Idade = dataReader.SafeGetString("idade"),
                        Sexo = dataReader.SafeGetString("sexo"),
                        Descricao = dataReader.SafeGetString("descricao"),
                        Foto = dataReader.SafeGetString("foto"),
                        Latitude = dataReader.GetDouble("latitude"),
                        Longitude = dataReader.GetDouble("longitude")
                    };

                    cachorros.Add(cachorro);
                }

                await dataReader.CloseAsync();
                await _database.Connection.CloseAsync();

                return cachorros;
            }

            await dataReader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }

        public async Task Update(CachorroEditarViewModel viewModel)
        {
            string query = "UPDATE cachorros SET nome = @nome, raca = @raca, porte = @porte, idade = @idade, sexo = @sexo, descricao = @descricao, foto = @foto WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", viewModel.Id);
            command.Parameters.AddWithValue("@nome", viewModel.Nome);
            command.Parameters.AddWithValue("@raca", viewModel.Raca);
            command.Parameters.AddWithValue("@porte", viewModel.Porte);
            command.Parameters.AddWithValue("@idade", viewModel.Idade);
            command.Parameters.AddWithValue("@sexo", viewModel.Sexo);
            command.Parameters.AddWithValue("@descricao", viewModel.Descricao);

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

            await command.ExecuteNonQueryAsync();

            await _database.Connection.CloseAsync();
        }

        public async Task Delete(Guid cachorroId, Guid usuarioId)
        {
            string query = "DELETE FROM cachorros_curtidos WHERE cachorro_id = @id; DELETE FROM cachorros_ignorados WHERE cachorro_id = @id; DELETE FROM cachorros WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", cachorroId);

            await command.ExecuteNonQueryAsync();

            string countQuery = "SELECT COUNT(*) FROM cachorros WHERE usuario_id = @usuario_id";

            command.CommandText = countQuery;

            command.Parameters.AddWithValue("@usuario_id", usuarioId);

            int rowsAmount = Convert.ToInt32(await command.ExecuteScalarAsync());

            if (rowsAmount == 0)
            {
                string matchesQuery = "DELETE FROM matches WHERE usuario_id = @usuario_id";

                command.CommandText = matchesQuery;

                await command.ExecuteNonQueryAsync();
            }

            await _database.Connection.CloseAsync();
        }

        public async Task<List<CachorroProcurarViewModel>> Search(Guid passeadorId)
        {
            double latitude = 0, longitude = 0, distanciaMaxima = 0;

            string query = "SELECT latitude, longitude, distancia_maxima FROM passeadores WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, _database.Connection);

            command.Parameters.AddWithValue("@id", passeadorId);

            using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                while (await dataReader.ReadAsync())
                {
                    latitude = dataReader.GetDouble("latitude");
                    longitude = dataReader.GetDouble("longitude");
                    distanciaMaxima = dataReader.GetDouble("distancia_maxima");
                }
            }

            await dataReader.CloseAsync();

            string searchQuery = "SELECT * FROM cachorros WHERE NOT EXISTS (SELECT null FROM cachorros_curtidos WHERE cachorros.id = cachorros_curtidos.cachorro_id AND passeador_id = @id) AND NOT EXISTS (SELECT null FROM cachorros_ignorados WHERE cachorros.id = cachorros_ignorados.cachorro_id AND passeador_id = @id)";

            command.CommandText = searchQuery;

            using MySqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                var cachorros = new List<CachorroProcurarViewModel>();

                while (await reader.ReadAsync())
                {
                    double distance = Geography.GetDistance(reader.GetDouble("latitude"),
                                                            reader.GetDouble("longitude"),
                                                            latitude, longitude);

                    if (distance <= distanciaMaxima)
                    {
                        var cachorro = new CachorroProcurarViewModel
                        {
                            Id = reader.GetGuid("id"),
                            UsuarioId = reader.GetGuid("usuario_id"),
                            Nome = reader.GetString("nome"),
                            Raca = reader.SafeGetString("raca"),
                            Porte = reader.SafeGetString("porte"),
                            Idade = reader.SafeGetString("idade"),
                            Sexo = reader.SafeGetString("sexo"),
                            Descricao = reader.SafeGetString("descricao"),
                            Foto = reader.SafeGetString("foto"),
                            Distancia = distance.ToString("0.##") + " km"
                        };

                        cachorros.Add(cachorro);
                    }
                }

                await reader.CloseAsync();
                await _database.Connection.CloseAsync();

                return cachorros;
            }

            await reader.CloseAsync();
            await _database.Connection.CloseAsync();

            return null;
        }
    }
}