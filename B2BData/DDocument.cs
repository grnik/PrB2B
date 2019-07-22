using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DDocument
    {
        #region Properties
        public int EntryNo { get; set; }
        public string CustomerNo { get; set; }
        public DateTime PostingDate { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public bool Open { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public string ExternalDocumentNo { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingAmount { get; set; }
        #endregion

        #region Constructores

        public DDocument() { }

        internal DDocument(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            /*
    select [Customer No_] CustomerNo, STA.Code, (STA.Name+STA.[Name 2]) Name
        , (STA.Address + STA.[Address 2]) Address, STA.City, STA.Contact, STA.[Phone No_] Phone
        , [Shipment Method Code] ShipmentMethodCode, SM.Description ShipmentMethod
        , [Shipping Agent Code] ShippingAgentCode, SA.Name ShippingAgent
        , STA.[Post Code] PostCode, STA.Note
        , STA.[Post Code Recipient] PostCodeRecipient, [City Recipient] CityRecipient, [Address Recipient] AddressRecipient         
             */
            EntryNo = Convert.ToInt32(reader["EntryNo"].ToString());
            CustomerNo = reader["CustomerNo"].ToString();
            PostingDate = Convert.ToDateTime(reader["PostingDate"]);
            DocumentType = reader["DocumentType"].ToString();
            DocumentNo = reader["DocumentNo"].ToString();
            Description = reader["Description"].ToString();
            CurrencyCode = reader["CurrencyCode"].ToString();
            Open = Convert.ToBoolean(reader["Open"]);
            DueDate = Convert.ToDateTime(reader["DueDate"]);
            DocumentDate = Convert.ToDateTime(reader["DocumentDate"]);
            ExternalDocumentNo = reader["ExternalDocumentNo"].ToString();
            Amount = Convert.ToDecimal(reader["Amount"]);
            RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]);
        }

        public static List<DDocument> GetByCustomerNo(string customerNo)
        {
            List<DDocument> res = new List<DDocument>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM [PronetB2B_CustLedgerEntry]
WHERE CustomerNo = @CustomerNo;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DDocument(data));
                    }
                }
            }

            return res;
        }

        public static List<DDocument> GetByCustPeriod(string customerNo, DateTime startDate, DateTime? finishDate)
        {
            finishDate = finishDate ?? DateTime.MaxValue;
            List<DDocument> res = new List<DDocument>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM [PronetB2B_CustLedgerEntry]
WHERE CustomerNo = @CustomerNo
  AND DocumentDate >= @StartDate
  AND DocumentDate <= @FinishDate
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                command.Parameters.Add("StartDate", SqlDbType.DateTime).Value = startDate;
                command.Parameters.Add("FinishDate", SqlDbType.DateTime).Value = finishDate;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DDocument(data));
                    }
                }
            }

            return res;
        }

        public static List<DDocument> GetPaymentSheduleByCustomerNo(string customerNo)
        {
            List<DDocument> res = new List<DDocument>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM [PronetB2B_CustLedgerEntry]
WHERE CustomerNo = @CustomerNo
  AND [Open] <> 0
  AND Amount > 0
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DDocument(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
