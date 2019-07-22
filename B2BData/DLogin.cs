using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace B2BData
{
    public class DLogin
    {
        #region Properties

        static DLogin Testing = new DLogin()
        {
            Email = "GrigoryevNE@pronetgroup.ru",
            Password = "123",
            Name = "Николай",
            Patronymic = "Евг",
            Surname = "Григ",
            Phone = "266",
            Token = "TEST_B2B_PRONET",
            LastVisitTime = DateTime.Now,
            CustomerNo = "TEST_GRIGORYEV"
        };

        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }

        public string Token { get; set; }
        public DateTime LastVisitTime { get; set; }

        public string CustomerNo { get; set; }

        public string SalespersonCode { get; set; }
        /// <summary>
        /// Логин может разрешать отгрузку из интернета
        /// </summary>
        public bool AllowShipmInt { get; set; }

        #endregion

        #region Constructores

        public DLogin() { }

        internal DLogin(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            Email = reader["Email"].ToString();
            Password = reader["Password"].ToString();
            Name = reader["Name"].ToString();
            Patronymic = reader["Patronymic"].ToString();
            Surname = reader["Surname"].ToString();
            Phone = reader["Phone"].ToString();
            Token = reader["Token"].ToString();
            LastVisitTime = (DateTime)reader["LastVisitTime"];
            CustomerNo = reader["CustomerNo"].ToString();
            SalespersonCode = reader["SalespersonCode"].ToString();
            AllowShipmInt = Convert.ToByte(reader["Allow Shipm_ Int_"]) != 0;
        }

        public static DLogin GetByLoginPass(string login, string password)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Login
WHERE Email = @Email
  AND Password = @Password;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Email", SqlDbType.VarChar).Value = login;
                command.Parameters.Add("Password", SqlDbType.VarChar).Value = password;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DLogin(data);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Присваиваем и сохраняем токен
        /// </summary>
        /// <returns></returns>
        public void SaveTokenLastVisitTime()
        {
            if (Token == Testing.Token)
            {
                Testing.LastVisitTime = DateTime.Now;
                return;
            }

            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
UPDATE PronetB2B_Login
SET Token = @Token
  , LastVisitTime = @LastVisitTime
WHERE Email = @Email;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Email", SqlDbType.VarChar).Value = Email;
                command.Parameters.Add("Token", SqlDbType.VarChar).Value = Token;
                command.Parameters.Add("LastVisitTime", SqlDbType.DateTime).Value = LastVisitTime;
                connect.Open();
                int count = command.ExecuteNonQuery();
                if (count != 1)
                    throw new Exception("При записи токена произошла ошибка. Число измененных строк не соответствует ожиданиям.");
            }
        }

        public static DLogin GetLoginByToken(string token)
        {
            if (token == Testing.Token)
            {
                Testing.LastVisitTime = DateTime.Now;
                return Testing;
            }

            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Login
WHERE Token = @Token;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Token", SqlDbType.VarChar).Value = token;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DLogin(data);
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
