using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DItemPrice
    {
        #region Properties

        public string ItemNo { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string CustomerNo { get; set; }

        #endregion

        #region Constructores

        public DItemPrice() { }

        internal DItemPrice(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            ItemNo = reader["ItemNo"].ToString();
            Price = reader["Price"] == DBNull.Value ? 0 : (decimal)reader["Price"];
            Currency = reader["Currency"].ToString();
            CustomerNo = reader["CustomerNo"].ToString();
        }

        public static DItemPrice GetByCustItemCurr(string customerNo, string id, string priceCurrency)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM ALL_PriceCustItemCurrencyMin
WHERE CustomerNo = @CustomerNo
  and ItemNo = @ItemNo
  and Currency = @Currency
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                command.Parameters.Add("ItemNo", SqlDbType.VarChar).Value = id;
                command.Parameters.Add("Currency", SqlDbType.VarChar).Value = priceCurrency;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DItemPrice(data);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static List<DItemPrice> GetAllByCustCurr(string customerNo, string priceCurrency)
        {
            List<DItemPrice> res = new List<DItemPrice>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
//                string commText = @"
//SELECT *
//FROM ALL_PriceCustItemCurrencyMin
//WHERE CustomerNo = @CustomerNo
//  and Currency = @Currency
//  and [dbo].[ItemAvailableCompany]([ItemNo], 'ГЛАВНЫЙ', 'RUR')<>0
//                ";
                string commText = @"
SELECT *
FROM PronetB2B_ItemPrices
WHERE CustomerNo = @CustomerNo
  and Currency = @Currency
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                command.Parameters.Add("Currency", SqlDbType.VarChar).Value = priceCurrency;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DItemPrice(data));
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Возвращаем только цену для данного товара и клиента
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="id"></param>
        /// <param name="priceCurrency"></param>
        /// <returns></returns>
        public static decimal GetPriceByCustItemCurr(string customerNo, string id, string priceCurrency)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT Price
FROM ALL_PriceCustItemCurrencyMin
WHERE CustomerNo = @CustomerNo
  and ItemNo = @ItemNo
  and Currency = @Currency
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                command.Parameters.Add("ItemNo", SqlDbType.VarChar).Value = id;
                command.Parameters.Add("Currency", SqlDbType.VarChar).Value = priceCurrency;
                connect.Open();
                var res = command.ExecuteScalar();
                return (res == null) || (res == DBNull.Value) ? 0 : (decimal)res;
            }
        }

        #endregion
    }
}
