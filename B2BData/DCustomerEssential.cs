using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DCustomerEssential
    {
        #region Properties

        public string CustomerNo { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string BankName { get; set; }
        public string BankCity { get; set; }
        public string BankAccountNo { get; set; }
        public string TransitNo { get; set; }
        public string BankCountryCode { get; set; }
        public string VATRegistrationNo { get; set; }
        public string PostCode { get; set; }
        public string BIC { get; set; }
        public string ActSignedByName { get; set; }
        public string ActSignedByPosition { get; set; }
        public string KPP { get; set; }
        public string OKPO { get; set; }
        public string OKONX { get; set; }
        public string AgreementNo { get; set; }
        public string ShipToCode { get; set; }
        public bool Credit { get; set; }
        public bool License { get; set; }
        public decimal InsuranceLimit { get; set; }
        public string Insurance { get; set; }

        #endregion Properties

        #region Constructores

        public DCustomerEssential() { }

        internal DCustomerEssential(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methords

        internal void Translate(SqlDataReader reader)
        {
            CustomerNo = reader["CustomerNo"].ToString();
            Id = reader["Id"].ToString();
            Name = reader["Name"].ToString();
            Address = reader["Address"].ToString();
            City = reader["City"].ToString();
            Phone = reader["Phone"].ToString();
            BankName = reader["BankName"].ToString();
            BankCity = reader["BankCity"].ToString();
            BankAccountNo = reader["BankAccountNo"].ToString();
            TransitNo = reader["TransitNo"].ToString();
            BankCountryCode = reader["BankCountryCode"].ToString();
            VATRegistrationNo = reader["VATRegistrationNo"].ToString();
            PostCode = reader["PostCode"].ToString();
            BIC = reader["BIC"].ToString();
            ActSignedByName = reader["ActSignedByName"].ToString();
            ActSignedByPosition = reader["ActSignedByPosition"].ToString();
            KPP = reader["KPP"].ToString();
            OKPO = reader["OKPO"].ToString();
            OKONX = reader["OKONX"].ToString();
            AgreementNo = reader["AgreementNo"].ToString();
            ShipToCode = reader["ShipToCode"].ToString();
            Credit = Convert.ToByte(reader["Credit"]) != 0;
            License = Convert.ToByte(reader["License"]) != 0;
            InsuranceLimit = (decimal)reader["InsuranceLimit"];
            Insurance = reader["Insurance"].ToString();
        }

        public static List<DCustomerEssential> GetByCustomerNo(string customerNo)
        {
            List<DCustomerEssential> res = new List<DCustomerEssential>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM PronetB2B_CustomerEssentials
WHERE CustomerNo = @CustomerNo;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CustomerNo", SqlDbType.VarChar).Value = customerNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DCustomerEssential(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
