using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using B2BLogical;

namespace PronetB2B.Controllers
{
    public class LoginController : ApiController
    {
        // http://png.pronetgroup.ru:116/api/
        // GET localhost:60088/api/Login/Logon?login=GrigoryevNE@pronetgroup.ru&password=123
        public LLogin GetLogon(string login, string password)
        {
            return B2BLogical.LLogin.LogOn(login, password);
        }
        public LSalesperson GetSalesperson(string token)
        {
            return B2BLogical.LSalesperson.GetByToken(token);
        }
    }
}
