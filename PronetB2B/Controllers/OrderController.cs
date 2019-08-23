using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using B2BLogical;
using NLog;

namespace PronetB2B.Controllers
{
    public class OrderController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // http://png.pronetgroup.ru:116/api/Order/GetOrders?Token=TEST_B2B_PRONET
        public List<LOrder> GetOrders(string token)
        {
            return B2BLogical.LOrder.GetByToken(token);
        }

        /*
         В Fiddler
POST http://localhost:60088/api/Order/ChangeOrder?token=TEST_B2B_PRONET

User-Agent: Fiddler
Host: localhost:60088
Content-Length: 181
Content-Type: application/json

            Request Body
{"Id":"{395A6BE4-444E-4334-AAF1-3FE8BCC0D687}","DocumentType":1,"DocumentNo":"КЛЗК-Р19-007417","LineNo":"10000","ItemNo":"505375"
,"Quantity":1.00,"Price":523,"Operation":0}
         */
        [System.Web.Http.HttpPost]
        public Guid ChangeOrder(string token, LOrderChange orderChange)
        {
            try
            {
                return orderChange.Insert(token);
            }
            catch (Exception exc)
            {
                logger.Error(exc);
                if (orderChange != null)
                    logger.Error(exc, "ChangeOrder " + orderChange.DocumentNo);
                throw;
            }
        }

        //http://png.pronetgroup.ru:116/api/Order/ChangeStatus?Token=TEST_B2B_PRONET&type=1&no=%D0%9A%D0%9B%D0%97%D0%9A-%D0%A019-010508&newStatus=1
        [System.Web.Http.HttpGet]
        public Guid ChangeStatus(string token, int type, string no, int newStatus)
        {
            try
            {
                LOrder order = LOrder.GetByNo(token, type, no);
                return order.ChangeStatus(token, newStatus);
            }
            catch (Exception exc)
            {
                logger.Error(exc);
                logger.Error(exc, string.Format("Token %0 ChangeStatus %1 %2 NewStatus %3", token, type, no, newStatus));
                throw;
            }
        }

        //http://localhost:60088/api/Order/GetStatus?token=TEST_B2B_PRONET&id=b64e1d7d-00ab-41a5-9ea1-910bf6698cc2
        [System.Web.Http.HttpGet]
        public LOrderStatus GetStatus(string token, string id)
        {
            return LOrderStatus.GetById(token, new Guid(id));
        }

        /*
         В Fiddler
POST http://localhost:60088/api/Customer/CreateOrder?token=TEST_B2B_PRONET

User-Agent: Fiddler
Host: localhost:60088
Content-Length: 676
Content-Type: application/json

            Request Body
{"EntryNo":26,"CustomerId":"TEST_GRIGORYEV","Comment":"","WantCheaper":"0","DeliveryPrice":0.00000000000000000000
           ,"DeliveryDays":0,"DeliveryServiceCode":"0","EssentialCode":"ПРОКОМП","CreateTime":"2017-05-30T07:00:46.507","Processed":true,"Processing":1
           ,"ProcessingTime":"2017-05-30T07:00:45.663","OrderId":"КЛЗК-П17-011945"
           ,"Line":[{
           "BasketNo":26,"LineNo":10000,"ItemNo":"PG267085","Quantity":1.00000000000000000000,"Price":2.63000000000000000000}
           ,{"BasketNo":26,"LineNo":20000,"ItemNo":"PG281904","Quantity":3.00000000000000000000,"Price":91.90000000000000000000}
           ,{"BasketNo":26,"LineNo":30000,"ItemNo":"PG280101","Quantity":3.00000000000000000000,"Price":656.50000000000000000000}]}
         */
        [System.Web.Http.HttpPost]
        public int CreateOrder(string token, LBasket basket)
        {
            try
            {
                return basket.Insert(token);
            }
            catch (Exception exc)
            {
                logger.Error(exc);
                if (basket != null)
                    logger.Error(exc, "CreateOrder " + basket.CustomerId);
                throw;
            }
        }

        //localhost:60088/api/Order/GetBasketOrder?Token=TEST_B2B_PRONET&no=24
        [System.Web.Http.HttpGet]
        public LBasket GetBasketOrder(string token, int no)
        {
            return LBasket.GetByNo(token, no);
        }

        //localhost:60088/api/Order/GetOrder?Token=TEST_B2B_PRONET&type=1&no=КЛЗК-Р19-004199
        [System.Web.Http.HttpGet]
        public LOrder GetOrder(string token, int type, string no)
        {
            return LOrder.GetByNo(token, type, no);
        }

    }
}
