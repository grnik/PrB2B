using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using B2BLogical;
using NLog;

namespace PronetB2B.Controllers
{
    public class CustomerController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

//        #region MoveToDocument

//        // http://png.pronetgroup.ru:116/api/Customer/GetDocuments?Token=TEST_B2B_PRONET
//        public List<LDocument> GetDocuments(string token)
//        {
//            return B2BLogical.LDocument.GetByToken(token);
//        }

//        // http://localhost:60088/api/Customer/GetDocuments?Token=TEST_B2B_PRONET&startDate=01.01.2018&finishDate=12.23.2018
//        public List<LDocument> GetDocuments(string token, DateTime startDate, DateTime? finishDate)
//        {
//            return B2BLogical.LDocument.GetByCustPeriod(token, startDate, finishDate);
//        }

//        // http://png.pronetgroup.ru:116/api/Customer/GetPaymentSchedule?Token=TEST_B2B_PRONET
//        public List<LDocument> GetPaymentSchedule(string token)
//        {
//            return B2BLogical.LDocument.GetPaymentSheduleByToken(token);
//        }

//        #endregion

//        #region MoveToOrder

//        /*
//         В Fiddler
//POST http://localhost:60088/api/Customer/CreateOrder?token=TEST_B2B_PRONET

//User-Agent: Fiddler
//Host: localhost:60088
//Content-Length: 676
//Content-Type: application/json

//            Request Body
//{"EntryNo":26,"CustomerId":"TEST_GRIGORYEV","Comment":"","WantCheaper":"0","DeliveryPrice":0.00000000000000000000
//           ,"DeliveryDays":0,"DeliveryServiceCode":"0","EssentialCode":"ПРОКОМП","CreateTime":"2017-05-30T07:00:46.507","Processed":true,"Processing":1
//           ,"ProcessingTime":"2017-05-30T07:00:45.663","OrderId":"КЛЗК-П17-011945"
//           ,"Line":[{
//           "BasketNo":26,"LineNo":10000,"ItemNo":"PG267085","Quantity":1.00000000000000000000,"Price":2.63000000000000000000}
//           ,{"BasketNo":26,"LineNo":20000,"ItemNo":"PG281904","Quantity":3.00000000000000000000,"Price":91.90000000000000000000}
//           ,{"BasketNo":26,"LineNo":30000,"ItemNo":"PG280101","Quantity":3.00000000000000000000,"Price":656.50000000000000000000}]}
//         */
//        [System.Web.Http.HttpPost]
//        public int CreateOrder(string token, LBasket basket)
//        {
//            try
//            {
//                return basket.Insert(token);
//            }
//            catch (Exception exc)
//            {
//                logger.Error(exc);
//                if (basket != null)
//                    logger.Error(exc, "CreateOrder " + basket.CustomerId);
//                throw;
//            }
//        }

//        //localhost:60088/api/Customer/GetBasketOrder?Token=TEST_B2B_PRONET&no=24
//        [System.Web.Http.HttpGet]
//        public LBasket GetBasketOrder(string token, int no)
//        {
//            return LBasket.GetByNo(token, no);
//        }

//        //localhost:60088/api/Customer/GetOrder?Token=TEST_B2B_PRONET&type=1&no=КЛЗК-Р19-004199
//        [System.Web.Http.HttpGet]
//        public LOrder GetOrder(string token, int type, string no)
//        {
//            return LOrder.GetByNo(token, type, no);
//        }

//        // http://png.pronetgroup.ru:116/api/Customer/GetOrders?Token=TEST_B2B_PRONET
//        public List<LOrder> GetOrders(string token)
//        {
//            return B2BLogical.LOrder.GetByToken(token);
//        }

//        #endregion
    }
}