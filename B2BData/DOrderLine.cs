using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DOrderLine
    {
        #region Properties
        public int DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public int LineNo { get; set; }
        public int Type { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public decimal HedgAmount { get; set; }
        public decimal DopFinance { get; set; }
        #endregion

        #region Constructors

        public DOrderLine() { }

        internal DOrderLine(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methods

        internal void Translate(SqlDataReader reader)
        {
            DocumentType = (int)reader["DocumentType"];
            DocumentNo = reader["DocumentNo"].ToString();
            LineNo = (int)reader["LineNo"];
            Type = (int)reader["Type"];
            No = reader["No"].ToString();
            Description = reader["Description"].ToString();
            Quantity = (decimal)reader["Quantity"];
            Price = (decimal)reader["Price"];
            Comment = reader["Comment"].ToString();
            HedgAmount = (decimal)reader["HedgAmount"];
            DopFinance = (decimal)reader["DopFinance"];
        }

        public static List<DOrderLine> GetByDocument(int documentType, string documentNo)
        {
            List<DOrderLine> res = new List<DOrderLine>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
  FROM [dbo].[PronetB2B_OrderLines]
  WHERE DocumentNo = @DocumentNo
    AND DocumentType = @DocumentType
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("DocumentType", SqlDbType.Int).Value = documentType;
                command.Parameters.Add("DocumentNo", SqlDbType.VarChar).Value = documentNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DOrderLine(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
