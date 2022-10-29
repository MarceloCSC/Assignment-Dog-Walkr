using MySqlConnector;

namespace DogWalkr.Utils
{
    public static class DbHelper
    {
        public static string DadosConexao => "Server=us-cdbr-east-06.cleardb.net;Database=heroku_5ccca09a9f74a29;Uid=b6cab22bbb1ff7;Pwd=7ae1da62;";

        public static string SafeGetString(this MySqlDataReader dataReader, string columnName)
        {
            if (!dataReader.IsDBNull(dataReader.GetOrdinal(columnName)))
            {
                return dataReader.GetString(columnName);
            }

            return string.Empty;
        }
    }
}