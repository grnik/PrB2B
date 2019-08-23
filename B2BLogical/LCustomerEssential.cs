using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace B2BLogical
{
    //http://localhost:60088/api/Customer/GetEssentials?Token=TEST_B2B_PRONET
    [DataContract]
    public class LCustomerEssential
    {
        #region Properties

        private static readonly MD5 Md5Hash = MD5.Create();

        [DataMember]
        public string Key
        {
            set { }
            get
            {
                string allString = CustomerNo + Id;
                return allString.GetKey();
            }
        }

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
        public string BIC { get; set; }
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
        /// <summary>
        /// Кредитный лимит по данным реквизитам
        /// </summary>
        [DataMember]
        public bool Credit { get; set; }
        [DataMember]
        public bool License { get; set; }
        /// <summary>
        /// Лимит страхования
        /// </summary>
        [DataMember]
        public decimal InsuranceLimit { get; set; }
        /// <summary>
        /// Кто дал лимит страхования
        /// </summary>
        [DataMember]
        public string Insurance { get; set; }
        [DataMember]
        public string DueDateCalculation { get; set; }
        [DataMember]
        public string CalendarCode { get; set; }
        [DataMember]
        public Decimal Balance { get; set; }
        /// <summary>
        /// Число просроченных документов. Оно показывает сколько документов с просрочкой есть на клиенте. Без учета юр.лица!
        /// </summary>
        [DataMember]
        public int OverdueDocuments { get; set; }

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
            DueDateCalculation = essential.DueDateCalculation;
            DueDateCalculation = DueDateCalculation.Replace("\u0002", "Д");
            CalendarCode = essential.CalendarCode;
            Balance = essential.Balance;
            OverdueDocuments = essential.OverdueDocuments;
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

        /// <summary>
        /// Генерация уникального ключа по коду клиента и ключу реквизитов
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static string GetKey(string customerNo, string id)
        {
            string input = customerNo + id;
            // Convert the input string to a byte array and compute the hash.
            byte[] data = Md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #endregion
    }
}
