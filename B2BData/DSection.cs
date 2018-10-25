using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DSection
    {
        #region Properties
        public string Code { get; set; }
        public string Description { get; set; }
        public int LowestLevel { get; set; }
        public string Parent { get; set; }
        public bool Popular { get; set; }
        #endregion Properties

        #region Constructores

        public DSection() { }

        internal DSection(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            Code = reader["Code"].ToString();
            Description = reader["Description"].ToString();
            LowestLevel = (int)reader["LowestLevel"];
            Parent = reader["Parent"].ToString();
            Popular = Convert.ToByte(reader["Popular"]) != 0;
        }

        public static List<DSection> GetAll()
        {
            List<DSection> res = new List<DSection>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Sections
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while(data.Read())
                    {
                        res.Add(new DSection(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
