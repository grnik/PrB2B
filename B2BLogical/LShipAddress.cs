using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    [DataContract]
    public class LShipAddress
    {
        #region Properties

        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string CustomerNo { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string ShipmentMethodCode { get; set; }
        [DataMember]
        public string ShipmentMethod { get; set; }
        [DataMember]
        public string ShippingAgentCode { get; set; }
        [DataMember]
        public string ShippingAgent { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string PostCodeRecipient { get; set; }
        [DataMember]
        public string CityRecipient { get; set; }
        [DataMember]
        public string AddressRecipient { get; set; }

        #endregion Properties

        #region Constructores

        internal LShipAddress(B2BData.DShipAddress shipAddress)
        {
            Translate(shipAddress);
        }

        #endregion

        #region Methords

        internal void Translate(DShipAddress shipAddress)
        {
            Code = shipAddress.Code;
            CustomerNo = shipAddress.CustomerNo;
            Name = shipAddress.Name;
            Address = shipAddress.Address;
            City = shipAddress.City;
            Phone = shipAddress.Phone;
            Contact = shipAddress.Contact;
            ShipmentMethodCode = shipAddress.ShipmentMethodCode;
            ShipmentMethod = shipAddress.ShipmentMethod;
            ShippingAgentCode = shipAddress.ShippingAgentCode;
            ShippingAgent = shipAddress.ShippingAgent;
            PostCode = shipAddress.PostCode;
            Note = shipAddress.Note;
            PostCodeRecipient = shipAddress.PostCodeRecipient;
            CityRecipient = shipAddress.CityRecipient;
        }

        internal static List<LShipAddress> Translate(List<DShipAddress> dShipAddresss)
        {
            List<LShipAddress> res = new List<LShipAddress>();
            foreach (DShipAddress dShipAddress in dShipAddresss)
            {
                res.Add(new LShipAddress(dShipAddress));
            }

            return res;
        }

        public static List<LShipAddress> GetByToken(string token)
        {
            LLogin login = LLogin.CheckToken(token);

            List<DShipAddress> shipAddresss = DShipAddress.GetByCustomerNo(login.CustomerNo);
            return shipAddresss != null ? Translate(shipAddresss) : null;
        }

        #endregion
    }
}
