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
    public class LDocument
    {
        #region Properties

        [DataMember]
        public int EntryNo { get; set; }
        [DataMember]
        public string CustomerNo { get; set; }
        [DataMember]
        public DateTime PostingDate { get; set; }
        [DataMember]
        public string DocumentType { get; set; }
        [DataMember]
        public string DocumentNo { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string CurrencyCode { get; set; }
        [DataMember]
        public bool Open { get; set; }
        [DataMember]
        public DateTime DueDate { get; set; }
        [DataMember]
        public DateTime DocumentDate { get; set; }
        [DataMember]
        public string ExternalDocumentNo { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal RemainingAmount { get; set; }

        #endregion Properties

        #region Constructores

        internal LDocument(B2BData.DDocument order)
        {
            Translate(order);
        }

        #endregion

        #region Methords

        internal void Translate(DDocument order)
        {
            DocumentType = order.DocumentType;
            EntryNo = order.EntryNo;
            CustomerNo = order.CustomerNo;
            PostingDate = order.PostingDate;
            DocumentType = order.DocumentType;
            DocumentNo = order.DocumentNo;
            Description = order.Description;
            CurrencyCode = order.CurrencyCode;
            Open = order.Open;
            DueDate = order.DueDate;
            DocumentDate = order.DocumentDate;
            ExternalDocumentNo = order.ExternalDocumentNo;
            Amount = order.Amount;
            RemainingAmount = order.RemainingAmount;
        }

        internal static List<LDocument> Translate(List<DDocument> dDocuments)
        {
            List<LDocument> res = new List<LDocument>();
            foreach (DDocument dDocument in dDocuments)
            {
                res.Add(new LDocument(dDocument));
            }

            return res;
        }

        public static List<LDocument> GetByToken(string token)
        {
            LLogin login = LLogin.CheckToken(token);

            List<DDocument> orders = DDocument.GetByCustomerNo(login.CustomerNo);
            return orders != null ? Translate(orders) : null;
        }

        public static List<LDocument> GetByCustPeriod(string token, DateTime startDate, DateTime? finishDate)
        {
            LLogin login = LLogin.CheckToken(token);

            List<DDocument> orders = DDocument.GetByCustPeriod(login.CustomerNo, startDate, finishDate);
            return orders != null ? Translate(orders) : null;
        }

        /// <summary>
        /// Список документов для оплаты
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static List<LDocument> GetPaymentSheduleByToken(string token)
        {
            LLogin login = LLogin.CheckToken(token);

            List<DDocument> orders = DDocument.GetPaymentSheduleByCustomerNo(login.CustomerNo);
            return orders != null ? Translate(orders) : null;
        }

        #endregion
    }
}
