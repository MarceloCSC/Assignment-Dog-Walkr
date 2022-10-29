using System;
using MySqlConnector;

namespace DogWalkr.Database
{
    public class DogWalkrDb : IDisposable
    {
        public MySqlConnection Connection { get; }

        public DogWalkrDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}