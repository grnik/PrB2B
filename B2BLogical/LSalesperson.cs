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
    public class LSalesperson
    {
        #region Properties

        [DataMember]
        public string EmailLogin { get; set; }
        [IgnoreDataMember]
        public string CustomerNo { get; set; }
        [IgnoreDataMember]
        public string Token { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string ICQ { get; set; }
        [DataMember]
        public string Mobile { get; set; }

        #endregion

        #region Constructores

        internal LSalesperson(B2BData.DSalesperson salesperson)
        {
            Translate(salesperson);
        }

        #endregion

        #region Methords

        internal void Translate(B2BData.DSalesperson salesperson)
        {
            EmailLogin = salesperson.EmailLogin;
            CustomerNo = salesperson.CustomerNo;
            Token = salesperson.Token;
            Code = salesperson.SalespersonCode;
            Name = salesperson.Name;
            Email = salesperson.Email;
            Phone = salesperson.Phone;
            ICQ = salesperson.ICQ;
            Mobile = salesperson.Mobile;
        }

        public static LSalesperson GetByToken(string token)
        {
            LLogin.CheckToken(token);

            DSalesperson salesperson = DSalesperson.GetSalespersonByToken(token);
            return salesperson != null ? new LSalesperson(salesperson) : null;
        }

        internal static LSalesperson GetByCode(string code)
        {
            DSalesperson salesperson = DSalesperson.GetByCode(code);
            return salesperson != null ? new LSalesperson(salesperson) : null;
        }

        #endregion
    }
}
