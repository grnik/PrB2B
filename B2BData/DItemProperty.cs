using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    /*
create view PronetB2B_ItemProperties
       as
       select IPV.[Item No_] ItemNo, IPV.[Item Group Property Code] GroupPropertyCode
       , IPV.[Item Property Code] Code, IPV.Value, IPV.[Order]
       , IP.[Type] Type, IP.Description
       from [Pronet2].[dbo].[RUR$Item Property Value] IPV
       inner join [Pronet2].[dbo].[RUR$Item Property] IP on IPV.[Item Property Code] = IP.Code
       */
    public class DItemProperty
    {
        #region Properties

        public string Code { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string ItemNo { get; set; }
        public string GroupPropertyCode { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }

        #endregion Properties

        #region Constructores

        public DItemProperty() { }

        internal DItemProperty(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            Code = reader["Code"].ToString();
            Description = reader["Description"].ToString();
            Type = (int)reader["Type"];
            ItemNo = reader["ItemNo"].ToString();
            GroupPropertyCode = reader["GroupPropertyCode"].ToString();
            Value = reader["Value"].ToString();
            Order = (int)reader["Order"];
        }

        public static List<DItemProperty> GetByItemNo(string itemNo)
        {
            List<DItemProperty> res = new List<DItemProperty>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_ItemProperties
WHERE ItemNo = @ItemNo
ORDER BY ItemNo, [Order]
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("ItemNo", SqlDbType.VarChar).Value = itemNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DItemProperty(data));
                    }
                }
            }

            return res;
        }

        #endregion

    }
}
