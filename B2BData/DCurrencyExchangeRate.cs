using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DCurrencyExchangeRate
    {
        #region Properties

        public string CurrencyCode { get; set; }
        public DateTime StartingDate { get; set; }
        public decimal ExchangeRate { get; set; }

        #endregion

        #region Constructores

        public DCurrencyExchangeRate() { }

        internal DCurrencyExchangeRate(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            CurrencyCode = reader["CurrencyCode"].ToString();
            StartingDate = (DateTime)reader["StartingDate"];
            ExchangeRate = (Decimal)reader["ExchangeRate"];
        }

        public static DCurrencyExchangeRate GetByLastCurrencyRate(string currency)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_CurrencyExchangeRates
WHERE CurrencyCode = @CurrencyCode
  AND [StartingDate] = (select max([StartingDate])
						FROM PronetB2B_CurrencyExchangeRates
						WHERE CurrencyCode = @CurrencyCode)
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CurrencyCode", SqlDbType.VarChar).Value = currency;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DCurrencyExchangeRate(data);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        #endregion

    }
}
