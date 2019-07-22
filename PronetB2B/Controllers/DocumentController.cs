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
    public class DocumentController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //http://localhost:60088/api/Document/GetInvoice?token=57ce61ac-6552-4b37-87fb-1583b17613c7&no=КЛСФ-Р19-008990
        [System.Web.Http.HttpGet]
        public LInvoice GetInvoice(string token, string no)
        {
            try
            {
                return LInvoice.GetByNo(token, no);
            }
            catch (Exception exc)
            {
                logger.Error(exc);
                if (exc.InnerException != null)
                    logger.Error(exc.InnerException);
                logger.Error(exc, "Token " + token + " GetInvoice " + no);
                throw;
            }
        }

        // http://png.pronetgroup.ru:116/api/Customer/GetDocuments?Token=TEST_B2B_PRONET
        public List<LDocument> GetDocuments(string token)
        {
            return B2BLogical.LDocument.GetByToken(token);
        }

        // http://localhost:60088/api/Customer/GetDocuments?Token=TEST_B2B_PRONET&startDate=01.01.2018&finishDate=12.23.2018
        public List<LDocument> GetDocuments(string token, DateTime startDate, DateTime? finishDate)
        {
            return B2BLogical.LDocument.GetByCustPeriod(token, startDate, finishDate);
        }

        // http://png.pronetgroup.ru:116/api/Customer/GetPaymentSchedule?Token=TEST_B2B_PRONET
        public List<LDocument> GetPaymentSchedule(string token)
        {
            return B2BLogical.LDocument.GetPaymentSheduleByToken(token);
        }
    }
}
