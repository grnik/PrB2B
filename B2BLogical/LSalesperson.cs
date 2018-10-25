using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public class LSalesperson
    {
        #region Properties

        public string EmailLogin { get; set; }
        public string CustomerNo { get; set; }
        public string Token { get; set; }
        public string SalespersonCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ICQ { get; set; }
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
            SalespersonCode = salesperson.SalespersonCode;
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

        #endregion
    }
}
