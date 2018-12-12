using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using B2BLogical;

namespace PronetB2B.Controllers
{
    public class CustomerController : ApiController
    {
        // GET
        // http://png.pronetgroup.ru:116/api/Customer/GetEssentials?Token=TEST_B2B_PRONET
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

        /*
         В Fiddler
POST http://localhost:60088/api/Customer/CreateOrder?token=TEST_B2B_PRONET

User-Agent: Fiddler
Host: localhost:60088
Content-Length: 87
Content-Type: application/json

            Request Body
{"EntryNo":"2","CompanyName":"RUR","CustomerId":"TEST_GRIGORYEV","Comment":"Test call"}
         */
        [System.Web.Http.HttpPost]
        public int CreateOrder(string token, LBasket basket)
        {
            return basket.Insert(token);
        }
        [System.Web.Http.HttpPost]
        public LBasket GetOrder(string token, int no)
        {
            return LBasket.GetByNo(token, no);
        }
    }
}