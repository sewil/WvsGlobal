using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;
using WvsBeta.Common.Enums;
using WvsBeta.Database;

namespace WvsBeta.Common.Packets
{
    public static class CashPacket
    {
        public static void AddTransactions(MySQL_Connection characterDb, int userId, Dictionary<TransactionType, List<(string reason, int amount)>> transactions)
        {
            characterDb.RunTransaction(x =>
            {
                var sb = new StringBuilder();

                sb.AppendLine("INSERT INTO user_point_transactions VALUES ");

                var start = true;
                foreach (var transaction in transactions)
                {

                    var boughtType = "";
                    switch (transaction.Key)
                    {
                        case TransactionType.MaplePoints: boughtType = "maplepoints"; break;
                        case TransactionType.NX: boughtType = "nx"; break;
                    }

                    foreach (var record in transaction.Value)
                    {
                        if (!start) sb.Append(',');
                        start = false;

                        sb.AppendFormat("(NULL, {0}, {1}, NOW(), '{2}', '{3}')\r\n", userId, -record.amount, MySqlHelper.EscapeString(record.reason), boughtType);
                    }
                }

                x.CommandText = sb.ToString();
                x.ExecuteNonQuery();
            });
        }
    }
}
