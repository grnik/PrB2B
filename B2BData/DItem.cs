using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public struct DItemAvailable
    {
        public string Id;
        public decimal Available;
    }

    public class DItem
    {
        #region Properties

        public string Id { get; set; }
        public string Name { get; set; }
        public string NameRus { get; set; }
        public string PartNumber { get; set; }
        public string Model { get; set; }
        public string EAN { get; set; }
        public bool Popular { get; set; }
        public bool Profitable { get; set; }
        public bool NewIncome { get; set; }
        public string ParentCode { get; set; }
        public decimal Available { get; set; }
        public decimal Weight { get; set; }
        public string BalanceType { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public string PriceType { get; set; }
        public string Guarantee { get; set; }

        #endregion

        #region Constructores

        public DItem() { }

        internal DItem(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            Id = reader["Id"].ToString();
            Name = reader["Name"].ToString();
            NameRus = reader["NameRus"].ToString();
            PartNumber = reader["PartNumber"].ToString();
            Model = reader["Model"].ToString();
            EAN = reader["EAN"].ToString();
            Popular = Convert.ToByte(reader["Popular"]) != 0;
            Profitable = Convert.ToByte(reader["Profitable"]) != 0;
            NewIncome = Convert.ToByte(reader["NewIncome"]) != 0;
            ParentCode = reader["ParentCode"].ToString();
            Available = (decimal)reader["Available"];
            Weight = (decimal)reader["Weight"];
            BalanceType = reader["BalanceType"].ToString();
            Volume = (decimal)reader["Volume"];
            Price = (decimal)reader["Price"];
            PriceCurrency = reader["PriceCurrency"].ToString();
            PriceType = reader["PriceType"].ToString();
            Guarantee = reader["Guarantee"].ToString();

        }

        /// <summary>
        /// По коду товара возвращаем его доступность.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static decimal GetAvailableById(string itemId)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT TOP 1 [Available]
FROM PronetB2B_Items
WHERE Id = @ItemId
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("ItemId", SqlDbType.VarChar).Value = itemId;
                connect.Open();
                return (decimal)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Возвращаем список доступного товара и его кол-во
        /// </summary>
        /// <returns></returns>
        public static List<DItemAvailable> GetAvailables()
        {
            List<DItemAvailable> result = new List<DItemAvailable>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT DISTINCT Id, [Available]
FROM PronetB2B_Items
WHERE [Available]  > 0
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        result.Add(new DItemAvailable() { Id = data["Id"].ToString(), Available = (decimal)data["Available"] });
                    }
                }
            }

            return result;
        }

        public static DItem GetByPriceTypeId(string priceType, string id)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Items
WHERE PriceType = @PriceType
  and Id = @Id
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("PriceType", SqlDbType.VarChar).Value = priceType;
                command.Parameters.Add("Id", SqlDbType.VarChar).Value = id;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DItem(data);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static List<DItem> GetAllByPriceType(string priceType, bool available = true)
        {
            List<DItem> res = new List<DItem>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Items
WHERE PriceType = @PriceType
                ";
                if (available)
                {
                    commText += " AND [Available]  > 0";
                }
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("PriceType", SqlDbType.VarChar).Value = priceType;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DItem(data));
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Возвращаем товар указанной группы
        /// </summary>
        /// <param name="priceType">Тип цены</param>
        /// <param name="parent">Родительская группа</param>
        /// <param name="withChildren">С подгруппами</param>
        /// <param name="available">Выдавать только доступный товар</param>
        /// <returns></returns>
        public static List<DItem> GetByParent(string priceType, string parent, bool withChildren, bool available = true)
        {
            List<DItem> res = new List<DItem>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Items
WHERE PriceType = @PriceType
  AND ParentCode like @ParentCode
                ";
                if (available)
                {
                    commText += " AND [Available]  > 0";
                }
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("PriceType", SqlDbType.VarChar).Value = priceType;
                string parentSearch = withChildren ? parent + "%" : parent;
                command.Parameters.Add("ParentCode", SqlDbType.VarChar).Value = parentSearch;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DItem(data));
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Возвращаем код товар указанной группы
        /// </summary>
        /// <param name="parent">Родительская группа</param>
        /// <param name="withChildren">С подгруппами</param>
        /// <param name="available">Выдавать только доступный товар</param>
        /// <returns></returns>
        public static List<string> GetItemIdByParent(string parent, bool withChildren, bool available = true)
        {
            List<string> res = new List<string>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                //Используем DISTINCT, что бы не писать PriceType = '1'
                string commText = @"
SELECT DISTINCT Id
FROM PronetB2B_Items
WHERE ParentCode like @ParentCode
                ";
                if (available)
                {
                    commText += " AND [Available]  > 0";
                }
                SqlCommand command = new SqlCommand(commText, connect);
                //command.Parameters.Add("PriceType", SqlDbType.VarChar).Value = priceType;
                string parentSearch = withChildren ? parent + "%" : parent;
                command.Parameters.Add("ParentCode", SqlDbType.VarChar).Value = parentSearch;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(data["Id"].ToString());
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
