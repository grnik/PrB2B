using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using B2BLogical;

namespace PronetB2B.Controllers
{
    public class CustomerController : ApiController
    {
        // GET
        // http://png.pronetgroup.ru:116/api/Customer/Essentials?Token=TEST_B2B_PRONET
        public List<LCustomerEssential> GetEssentials(string token)
        {
            return B2BLogical.LCustomerEssential.GetByToken(token);
        }

        // GET
        // http://png.pronetgroup.ru:116/api/Customer/GetShipAddresses?Token=TEST_B2B_PRONET
        public List<LShipAddress> GetShipAddresses(string token)
        {
            return B2BLogical.LShipAddress.GetByToken(token);
        }

        // http://png.pronetgroup.ru:116/api/Customer/GetOrders?Token=TEST_B2B_PRONET
        public List<LOrder> GetOrders(string token)
        {
            return B2BLogical.LOrder.GetByToken(token);
        }

        // http://png.pronetgroup.ru:116/api/Customer/GetDocuments?Token=TEST_B2B_PRONET
        public List<LDocument> GetDocuments(string token)
        {
            return B2BLogical.LDocument.GetByToken(token);
        }
    }
}