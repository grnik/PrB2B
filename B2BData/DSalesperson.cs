using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace B2BData
{
    public class DSalesperson
    {
        #region Properties

        public string EmailLogin { get; set; }
        public string CustomerNo { get; set; }
        public string Token { get; set; }
        public string SalespersonCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ICQ { get; set; }
        public string Mobile { get; set; }

        #endregion

        #region Constructores

        public DSalesperson() { }

        internal DSalesperson(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            EmailLogin = reader["EmailLogin"].ToString();
            CustomerNo = reader["CustomerNo"].ToString();
            Token = reader["Token"].ToString();
            SalespersonCode = reader["SalespersonCode"].ToString();
            Name = reader["Name"].ToString();
            Email = reader["Email"].ToString();
            Phone = reader["Phone"].ToString();
            ICQ = reader["ICQ"].ToString();
            Mobile = reader["Mobile"].ToString();
        }

        public static DSalesperson GetSalespersonByToken(string token)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Salesperson
WHERE Token = @Token;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Token", SqlDbType.VarChar).Value = token;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DSalesperson(data);
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
