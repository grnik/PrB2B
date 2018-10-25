using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;
using System.Runtime.Serialization;

namespace B2BLogical
{
    //http://localhost:60088/api/Customer/Essentials?Token=b6ea29b8-b07f-450e-8e2f-c2f86137dff4
    [DataContract]
    public class LCustomerEssential
    {
        #region Properties

        [DataMember]
        public string CustomerNo { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string BankCity { get; set; }
        [DataMember]
        public string BankAccountNo { get; set; }
        [DataMember]
        public string TransitNo { get; set; }
        [DataMember]
        public string BankCountryCode { get; set; }
        [DataMember]
        public string VATRegistrationNo { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string BIC { get; set; }
        [DataMember]
        public string ActSignedByName { get; set; }
        [DataMember]
        public string ActSignedByPosition { get; set; }
        [DataMember]
        public string KPP { get; set; }
        [DataMember]
        public string OKPO { get; set; }
        [DataMember]
        public string OKONX { get; set; }
        [DataMember]
        public string AgreementNo { get; set; }
        [DataMember]
        public string ShipToCode { get; set; }
        [DataMember]
        public bool Credit { get; set; }
        [DataMember]
        public bool License { get; set; }
        [DataMember]
        public decimal InsuranceLimit { get; set; }
        [DataMember]
        public string Insurance { get; set; }

        #endregion Properties

        #region Constructores

        internal LCustomerEssential(B2BData.DCustomerEssential essential)
        {
            Translate(essential);
        }

        #endregion

        #region Methords

        internal void Translate(DCustomerEssential essential)
        {
            CustomerNo = essential.CustomerNo;
            Id = essential.Id;
            Name = essential.Name;
            Address = essential.Address;
            City = essential.City;
            Phone = essential.Phone;
            BankName = essential.BankName;
            BankCity = essential.BankCity;
            BankAccountNo = essential.BankAccountNo;
            TransitNo = essential.TransitNo;
            BankCountryCode = essential.BankCountryCode;
            VATRegistrationNo = essential.VATRegistrationNo;
            PostCode = essential.PostCode;
            BIC = essential.BIC;
            ActSignedByName = essential.ActSignedByName;
            ActSignedByPosition = essential.ActSignedByPosition;
            KPP = essential.KPP;
            OKPO = essential.OKPO;
            OKONX = essential.OKONX;
            AgreementNo = essential.AgreementNo;
            ShipToCode = essential.ShipToCode;
            Credit = essential.Credit;
            License = essential.License;
            InsuranceLimit = essential.InsuranceLimit;
            Insurance = essential.Insurance;
        }

        internal static List<LCustomerEssential> Translate(List<DCustomerEssential> dCustomerEssentials)
        {
            List<LCustomerEssential> res = new List<LCustomerEssential>();
            foreach (DCustomerEssential dCustomerEssential in dCustomerEssentials)
            {
                res.Add(new LCustomerEssential(dCustomerEssential));
            }

            return res;
        }

        public static List<LCustomerEssential> GetByToken(string token)
        {
            LLogin login = LLogin.CheckToken(token);

            List<DCustomerEssential> essentials = DCustomerEssential.GetByCustomerNo(login.CustomerNo);
            return essentials != null ? Translate(essentials) : null;
        }

        #endregion
    }
}
