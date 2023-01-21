using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Database;

namespace WvsBeta.Common.Packets
{
    public static class CashPacket
    {
        public static void AddTransactions(MySQL_Connection characterDb, IList<ShopTransaction> transactions)
        {
            characterDb.RunTransaction(x =>
            {
                var sb = new StringBuilder();

                sb.AppendLine("INSERT INTO user_point_transactions VALUES ");

                var start = true;
                foreach (var transaction in transactions)
                {
                    var pointtype = transaction.type == TransactionType.MaplePoints ? "maplepoints" : "nx";
                    if (!start) sb.Append(',');
                    start = false;
                    sb.AppendFormat("(NULL, {0}, {1}, NOW(), {2}, '{3}', '{4}')\r\n", transaction.userid, -transaction.amount, transaction.sn, MySqlHelper.EscapeString(transaction.note), pointtype);
                }

                x.CommandText = sb.ToString();
                x.ExecuteNonQuery();
            });
        }
    }
}
