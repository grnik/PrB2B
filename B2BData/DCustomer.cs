using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DCustomer
    {
        #region Properties

        public string Id { get; set; }
        public string CommonNo { get; set; }
        public string Name { get; set; }
        public string PriceType { get; set; }
        public string BalanceType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Currency { get; set; }
         /// <summary>
        /// Число просроченных документов
        /// </summary>
        public int OverdueDocuments { get; set; }
        public string SalespersonCode { get; set; }
        public string SalespersonCodeNB { get; set; }
        public string SalespersonCodeRegional { get; set; }

        #endregion

        #region Constructores

        public DCustomer() { }

        internal DCustomer(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            Id = reader["Id"].ToString();
            CommonNo = reader["CommonNo"].ToString();
            Name = reader["Name"].ToString();
            PriceType = reader["PriceType"].ToString();
            BalanceType = reader["BalanceType"].ToString();
            Phone = reader["Phone"].ToString();
            Email = reader["Email"].ToString();
            Currency = reader["Currency"].ToString();
            OverdueDocuments = (int)reader["OverdueDocuments"];
            SalespersonCode = reader["SalespersonCode"].ToString();
            SalespersonCodeNB = reader["SalespersonCodeNB"].ToString();
            SalespersonCodeRegional = reader["SalespersonCodeRegional"].ToString();
        }

        public static List<DCustomer> GetAll()
        {
            List<DCustomer> res = new List<DCustomer>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Customers
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DCustomer(data));
                    }
                }
            }

            return res;
        }

        public static DCustomer GetById(string id)
        {
            List<DCustomer> res = new List<DCustomer>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Customers
WHERE Id = @Id
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Id", SqlDbType.VarChar).Value = id;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DCustomer(data);
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
