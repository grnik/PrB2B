using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DShipAddress
    {
        #region Properties

        public string Code { get; set; }
        public string CustomerNo { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string ShipmentMethodCode { get; set; }
        public string ShipmentMethod { get; set; }
        public string ShippingAgentCode { get; set; }
        public string ShippingAgent { get; set; }
        public string PostCode { get; set; }
        public string Note { get; set; }
        public string PostCodeRecipient { get; set; }
        public string CityRecipient { get; set; }
        public string AddressRecipient { get; set; }

        #endregion Properties

        #region Constructores

        public DShipAddress() { }

        internal DShipAddress(SqlDataReader reader)
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
            CustomerNo = reader["CustomerNo"].ToString();
            Code = reader["Code"].ToString();
            Name = reader["Name"].ToString();
            Address = reader["Address"].ToString();
            City = reader["City"].ToString();
            Phone = reader["Phone"].ToString();
            Contact = reader["Contact"].ToString();
            ShipmentMethodCode = reader["ShipmentMethodCode"].ToString();
            ShipmentMethod = reader["ShipmentMethod"].ToString();
            ShippingAgentCode = reader["ShippingAgentCode"].ToString();
            ShippingAgent = reader["ShippingAgent"].ToString();
            PostCode = reader["PostCode"].ToString();
            Note = reader["Note"].ToString();
            PostCodeRecipient = reader["PostCodeRecipient"].ToString();
            CityRecipient = reader["CityRecipient"].ToString();
            AddressRecipient = reader["AddressRecipient"].ToString();
        }

        public static List<DShipAddress> GetByCustomerNo(string customerNo)
        {
            List<DShipAddress> res = new List<DShipAddress>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_ShipAddresses
WHERE CustomerNo = @CustomerNo;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DShipAddress(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
