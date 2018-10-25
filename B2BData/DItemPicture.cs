using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DItemPicture
    {
        #region Properties

        public string Code { get; set; }
        public int Type { get; set; }
        public string FileName { get; set; }
        public string ItemNo { get; set; }

        #endregion Properties

        #region Constructores

        public DItemPicture() { }

        internal DItemPicture(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            Code = reader["Code"].ToString();
            FileName = reader["FileName"].ToString();
            Type = (int)reader["Type"];
            ItemNo = reader["ItemNo"].ToString();
        }

        public static List<DItemPicture> GetByItemNo(string itemNo)
        {
            List<DItemPicture> res = new List<DItemPicture>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_ItemPictures
WHERE ItemNo = @ItemNo
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("ItemNo", SqlDbType.VarChar).Value = itemNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DItemPicture(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
