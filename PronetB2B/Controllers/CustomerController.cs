using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using B2BLogical;

namespace PronetB2B.Controllers
{
    public class CustomerController : ApiController
    {
        // GET
        // http://png.pronetgroup.ru:116/api/Customer/Essentials?Token=ssssss
        public List<LCustomerEssential> GetEssentials(string token)
        {
            return B2BLogical.LCustomerEssential.GetByToken(token);
        }

        // GET
        // http://png.pronetgroup.ru:116/api/Customer/Essentials?Token=ssssss
        public List<LShipAddress> GetShipAddresses(string token)
        {
            return B2BLogical.LShipAddress.GetByToken(token);
        }
    }
}