using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace B2BData
{
    public class DOrder
    {
        #region Properties

        public int DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string CustomerNo { get; set; }
        public string Comment { get; set; }
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
        public string ShippingAgentServiceCode { get; set; }
        public string LegalCompanyCode { get; set; }
        public string LegalCompanyName { get; set; }
        public string LegalCompanyCurrencyCode { get; set; }
        public decimal LegalCompanyCurrencyFactor { get; set; }
        public int OrderStatus { get; set; }
        public string EssentialCustomer { get; set; }
        public string ShippingNote { get; set; }
        public string DocumentReason { get; set; }
        public DateTime DeleteDate { get; set; }
        public string BaseCalendarCode { get; set; }
        public string AgreementType { get; set; }
        public string AccFactNumber { get; set; }
        public DateTime AccFactDate { get; set; }
        public string ActNumber { get; set; }
        public DateTime ActDate { get; set; }
        public decimal DopFinanceProc { get; set; }
        public decimal TotalSum { get; set; }
        #endregion

        #region Constructores

        public DOrder() { }

        internal DOrder(SqlDataReader reader)
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
            DocumentType = Convert.ToInt32(reader["DocumentType"]);
            DocumentNo = reader["DocumentNo"].ToString();
            CustomerNo = reader["CustomerNo"].ToString();
            Comment = reader["Comment"].ToString();
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
            ShippingAgentServiceCode = reader["Shipping Agent Service Code"].ToString();
            LegalCompanyCode = reader["LegalCompanyCode"].ToString();
            LegalCompanyName = reader["LegalCompanyName"].ToString();
            LegalCompanyCurrencyCode = reader["LegalCompanyCurrencyCode"].ToString();
            LegalCompanyCurrencyFactor = Convert.ToDecimal(reader["LegalCompanyCurrencyFactor"]);
            OrderStatus = Convert.ToInt32(reader["OrderStatus"]);
            EssentialCustomer = reader["EssentialCustomer"].ToString();
            ShippingNote = reader["Shipping Note"].ToString();
            DocumentReason = reader["Document Reason"].ToString();
            DeleteDate = Convert.ToDateTime(reader["Delete Date"]);
            BaseCalendarCode = reader["Base Calendar Code"].ToString();
            AgreementType = reader["Agreement Type"].ToString();
            AccFactNumber = reader["Acc_Fact_ Number"].ToString();
            AccFactDate = Convert.ToDateTime(reader["Acc_Fact_ Date"]);
            ActNumber = reader["Act Number"].ToString();
            ActDate = Convert.ToDateTime(reader["Act Date"]);
            DopFinanceProc = Convert.ToDecimal(reader["Dop_ Finance %"]);
            TotalSum = Convert.ToDecimal(reader["TotalSum"]);
        }

        public static List<DOrder> GetByCustomerNo(string customerNo)
        {
            List<DOrder> res = new List<DOrder>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_Orders
WHERE CustomerNo = @CustomerNo;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DOrder(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
