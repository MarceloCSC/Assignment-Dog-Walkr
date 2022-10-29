using MySqlConnector;

namespace DogWalkr.Utils
{
    public static class DbHelper
    {
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