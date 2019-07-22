using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DInvoice
    {
        #region Properties

        public string DocumentNo { get; set; }
        public string CustomerNo { get; set; }
        public string Comment { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string PaymentTermsCode { get; set; }
        public DateTime DueDate { get; set; }
        public string ShipmentMethodCode { get; set; }
        public string LocationCode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyFactor { get; set; }
        public string SalespersonCode { get; set; }
        public string ShipToPostCode { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress { get; set; }
        public DateTime DocumentDate { get; set; }
        public string ShippingAgentCode { get; set; }
        public string LegalCompanyCode { get; set; }
        public string LegalCompanyName { get; set; }
        public string LegalCompanyCurrencyCode { get; set; }
        public decimal LegalCompanyCurrencyFactor { get; set; }
        public string EssentialCustomer { get; set; }
        public string EssentialCustomerDel { get; set; }
        public string ShippingNote { get; set; }
        public string DocumentReason { get; set; }
        public string BaseCalendarCode { get; set; }
        public string AgreementType { get; set; }
        public string AccFactNumber { get; set; }
        public DateTime AccFactDate { get; set; }
        public string ActNumber { get; set; }
        public DateTime ActDate { get; set; }
        public decimal DopFinanceProc { get; set; }
        public decimal TotalSum { get; set; }
        public decimal InCredit { get; set; }
        public decimal AccountSum { get; set; }
        #endregion

        #region Constructores

        public DInvoice() { }

        internal DInvoice(SqlDataReader reader)
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
            DocumentNo = reader["DocumentNo"].ToString();
            CustomerNo = reader["CustomerNo"].ToString();
            Comment = reader["Comment"].ToString();
            OrderNo = reader["OrderNo"].ToString();
            OrderDate = Convert.ToDateTime(reader["OrderDate"]);
            ShipmentDate = Convert.ToDateTime(reader["ShipmentDate"]);
            PaymentTermsCode = reader["PaymentTermsCode"].ToString();
            DueDate = Convert.ToDateTime(reader["DueDate"]);
            ShipmentMethodCode = reader["ShipmentMethodCode"].ToString();
            LocationCode = reader["LocationCode"].ToString();
            CurrencyCode = reader["CurrencyCode"].ToString();
            CurrencyFactor = Convert.ToDecimal(reader["CurrencyFactor"]);
            SalespersonCode = reader["SalespersonCode"].ToString();
            ShipToPostCode = reader["Ship-to Post Code"].ToString();
            ShipToCity = reader["Ship-to City"].ToString();
            ShipToName = reader["Ship-to Name"].ToString();
            ShipToAddress = reader["Ship-to Address"].ToString();
            DocumentDate = Convert.ToDateTime(reader["DocumentDate"]);
            ShippingAgentCode = reader["Shipping Agent Code"].ToString();
            LegalCompanyCode = reader["LegalCompanyCode"].ToString();
            LegalCompanyName = reader["LegalCompanyName"].ToString();
            LegalCompanyCurrencyCode = reader["LegalCompanyCurrencyCode"].ToString();
            LegalCompanyCurrencyFactor = Convert.ToDecimal(reader["LegalCompanyCurrencyFactor"]);
            EssentialCustomer = reader["EssentialCustomer"].ToString();
            ShippingNote = reader["Shipping Note"].ToString();
            DocumentReason = reader["Document Reason"].ToString();
            BaseCalendarCode = reader["Base Calendar Code"].ToString();
            AgreementType = reader["Agreement Type"].ToString();
            AccFactNumber = reader["Acc_Fact_ Number"].ToString();
            AccFactDate = Convert.ToDateTime(reader["Acc_Fact_ Date"]);
            ActNumber = reader["Act Number"].ToString();
            ActDate = Convert.ToDateTime(reader["Act Date"]);
            DopFinanceProc = Convert.ToDecimal(reader["Dop_ Finance %"]);
            TotalSum = reader["TotalSum"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalSum"]);
            InCredit = reader["In Credit"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["In Credit"]);
            AccountSum = reader["Account Sum"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Account Sum"]);
        }

        public static List<DInvoice> GetByCustomerNo(string customerNo)
        {
            List<DInvoice> res = new List<DInvoice>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Invoices
WHERE CustomerNo = @CustomerNo;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DInvoice(data));
                    }
                }
            }

            return res;
        }

        public static DInvoice GetByNo(string no)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Invoices
WHERE DocumentNo = @DocumentNo
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("DocumentNo", SqlDbType.VarChar).Value = no;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DInvoice(data);
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
