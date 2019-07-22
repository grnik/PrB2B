using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DInvoiceLine
    {
        #region Properties
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

        public DInvoiceLine() { }

        internal DInvoiceLine(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methods

        internal void Translate(SqlDataReader reader)
        {
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

        public static List<DInvoiceLine> GetByDocument(string documentNo)
        {
            List<DInvoiceLine> res = new List<DInvoiceLine>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
  FROM [dbo].[PronetB2B_InvoiceLines]
  WHERE DocumentNo = @DocumentNo
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("DocumentNo", SqlDbType.VarChar).Value = documentNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DInvoiceLine(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
