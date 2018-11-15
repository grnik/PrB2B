using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
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
        /// 
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
                if(available)
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

        #endregion

    }
}
