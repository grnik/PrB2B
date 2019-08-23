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
    public class LInvoice
    {
        #region Properties

        [DataMember]
        public string DocumentNo { get; set; }
        [DataMember]
        public string CustomerNo { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string OrderNo { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public DateTime ShipmentDate { get; set; }
        [DataMember]
        public string PaymentTermsCode { get; set; }
        [DataMember]
        public DateTime DueDate { get; set; }
        [DataMember]
        public string ShipmentMethodCode { get; set; }
        [DataMember]
        public string LocationCode { get; set; }
        [DataMember]
        public string CurrencyCode { get; set; }
        [DataMember]
        public decimal CurrencyFactor { get; set; }
        [DataMember]
        public string SalespersonCode { get; set; }
        [DataMember]
        public string ShipToPostCode { get; set; }
        [DataMember]
        public string ShipToCity { get; set; }
        [DataMember]
        public string ShipToName { get; set; }
        [DataMember]
        public string ShipToAddress { get; set; }
        [DataMember]
        public DateTime DocumentDate { get; set; }
        [DataMember]
        public string ShippingAgentCode { get; set; }
        [DataMember]
        public string LegalCompanyCode { get; set; }
        [DataMember]
        public string LegalCompanyName { get; set; }
        [DataMember]
        public string LegalCompanyCurrencyCode { get; set; }
        [DataMember]
        public decimal LegalCompanyCurrencyFactor { get; set; }
        [DataMember]
        public string EssentialCustomer { get; set; }
        [DataMember]
        public string EssentialCustomerDel { get; set; }
        [DataMember]
        public string EssentialCustomerKey
        {
            get
            {
                string key = CustomerNo + EssentialCustomer;
                return string.IsNullOrEmpty(EssentialCustomer) ? string.Empty : key.GetKey();
            }
            private set { throw new NotSupportedException("Метод только на чтение"); }
        }
        [DataMember]
        public string ShippingNote { get; set; }
        [DataMember]
        public string DocumentReason { get; set; }
        [IgnoreDataMember]
        public DateTime DeleteDate { get; set; }
        [IgnoreDataMember]
        public string BaseCalendarCode { get; set; }
        [DataMember]
        public string AgreementType { get; set; }
        [DataMember]
        public string AccFactNumber { get; set; }
        [DataMember]
        public DateTime AccFactDate { get; set; }
        [DataMember]
        public string ActNumber { get; set; }
        [DataMember]
        public DateTime ActDate { get; set; }
        [IgnoreDataMember]
        public decimal DopFinanceProc { get; set; }
        [DataMember]
        public decimal TotalSum { get; set; }
        [DataMember]
        public decimal InCredit { get; set; }
        [DataMember]
        public decimal AccountSum { get; set; }
        /// <summary>
        /// Строки заказа
        /// </summary>
        [DataMember]
        public List<LInvoiceLine> Line { get; set; }

        #endregion Properties

        #region Constructores

        internal LInvoice(B2BData.DInvoice order)
        {
            Translate(order);
        }

        #endregion

        #region Methords

        internal void Translate(DInvoice order)
        {
            DocumentNo = order.DocumentNo;
            CustomerNo = order.CustomerNo;
            Comment = order.Comment;
            OrderNo = order.OrderNo;
            OrderDate = order.OrderDate;
            ShipmentDate = order.ShipmentDate;
            PaymentTermsCode = order.PaymentTermsCode;
            DueDate = order.DueDate;
            ShipmentMethodCode = order.ShipmentMethodCode;
            LocationCode = order.LocationCode;
            CurrencyCode = order.CurrencyCode;
            CurrencyFactor = order.CurrencyFactor;
            SalespersonCode = order.SalespersonCode;
            ShipToPostCode = order.ShipToPostCode;
            ShipToCity = order.ShipToCity;
            ShipToName = order.ShipToName;
            ShipToAddress = order.ShipToAddress;
            DocumentDate = order.DocumentDate;
            ShippingAgentCode = order.ShippingAgentCode;
            LegalCompanyCode = order.LegalCompanyCode;
            LegalCompanyName = order.LegalCompanyName;
            LegalCompanyCurrencyCode = order.LegalCompanyCurrencyCode;
            LegalCompanyCurrencyFactor = order.LegalCompanyCurrencyFactor;
            EssentialCustomer = order.EssentialCustomer;
            EssentialCustomerDel = order.EssentialCustomerDel;
            ShippingNote = order.ShippingNote;
            DocumentReason = order.DocumentReason;
            BaseCalendarCode = order.BaseCalendarCode;
            AgreementType = order.AgreementType;
            AccFactNumber = order.AccFactNumber;
            AccFactDate = order.AccFactDate;
            ActNumber = order.ActNumber;
            ActDate = order.ActDate;
            DopFinanceProc = order.DopFinanceProc;
            TotalSum = order.TotalSum;
            InCredit = order.InCredit;
            AccountSum = order.AccountSum;

            Line = new List<LInvoiceLine>();
            List<DInvoiceLine> lines = DInvoiceLine.GetByDocument(DocumentNo);
            foreach (DInvoiceLine line in lines)
            {
                Line.Add(new LInvoiceLine(this, line));
            }
        }

        internal static List<LInvoice> Translate(List<DInvoice> dInvoices)
        {
            List<LInvoice> res = new List<LInvoice>();
            foreach (DInvoice dInvoice in dInvoices)
            {
                res.Add(new LInvoice(dInvoice));
            }

            return res;
        }

        public static List<LInvoice> GetByToken(string token)
        {
            LLogin login = LLogin.CheckToken(token);

            List<DInvoice> orders = DInvoice.GetByCustomerNo(login.CustomerNo);
            return orders != null ? Translate(orders) : null;
        }

        public static LInvoice GetByNo(string token, string no)
        {
            LLogin login = LLogin.CheckToken(token);
            //DCustomer customer = DCustomer.GetById(login.CustomerNo);

            DInvoice dInvoice = DInvoice.GetByNo(no);
            if (dInvoice == null)
                return null;

            if (dInvoice.CustomerNo != login.CustomerNo)
                return null;

            return new LInvoice(dInvoice);
        }

        #endregion
    }
}
