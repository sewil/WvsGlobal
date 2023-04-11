using MySql.Data.MySqlClient;

namespace WvsBeta.Common.Extensions
{
    public static class MySqlExtensions
    {
        public static T? GetNullableFieldValue<T>(this MySqlDataReader reader, string column) where T : struct
        {
            int ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal)) return null;
            return reader.GetFieldValue<T>(ordinal);
        }
    }
}
