using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    /// <summary>
    /// ENU=Reserv,Writed,Gather,Shipment;RUS=Резерв,Подтвержден,На сборку,К отгрузке
    /// </summary>
    public enum OrderStatus { Reserv = 0, Writed = 1, Gather = 2, Shipment = 3 }

    [DataContract]
    public class LOrder
    {
        #region Properties

        [DataMember]
        public int DocumentType { get; set; }
        [DataMember]
        public string DocumentNo { get; set; }
        [DataMember]
        public string CustomerNo { get; set; }
        [DataMember]
        public string Comment { get; set; }
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
        public string ShippingAgentServiceCode { get; set; }
        [DataMember]
        public string LegalCompanyCode { get; set; }
        [DataMember]
        public string LegalCompanyName { get; set; }
        [DataMember]
        public string LegalCompanyCurrencyCode { get; set; }
        [DataMember]
        public decimal LegalCompanyCurrencyFactor { get; set; }
        [DataMember]
        public int OrderStatus { get; set; }
        [DataMember]
        public string EssentialCustomer { get; set; }
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
        [DataMember]
        public DateTime DeleteDate { get; set; }
        [DataMember]
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
        [DataMember]
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
        public List<LOrderLine> Line { get; set; }

        #endregion Properties

        #region Constructores

        internal LOrder(B2BData.DOrder order)
        {
            Translate(order);
        }

        #endregion

        #region Methords

        internal void Translate(DOrder order)
        {
            DocumentType = order.DocumentType;
            DocumentNo = order.DocumentNo;
            CustomerNo = order.CustomerNo;
            Comment = order.Comment;
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
            ShippingAgentServiceCode = order.ShippingAgentServiceCode;
            LegalCompanyCode = order.LegalCompanyCode;
            LegalCompanyName = order.LegalCompanyName;
            LegalCompanyCurrencyCode = order.LegalCompanyCurrencyCode;
            LegalCompanyCurrencyFactor = order.LegalCompanyCurrencyFactor;
            OrderStatus = order.OrderStatus;
            EssentialCustomer = order.EssentialCustomer;
            ShippingNote = order.ShippingNote;
            DocumentReason = order.DocumentReason;
            DeleteDate = order.DeleteDate;
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

            Line = new List<LOrderLine>();
            List<DOrderLine> lines = DOrderLine.GetByDocument(DocumentType, DocumentNo);
            foreach (DOrderLine line in lines)
            {
                Line.Add(new LOrderLine(this, line));
            }
        }

        internal static List<LOrder> Translate(List<DOrder> dOrders)
        {
            List<LOrder> res = new List<LOrder>();
            foreach (DOrder dOrder in dOrders)
            {
                res.Add(new LOrder(dOrder));
            }

            return res;
        }

        public static List<LOrder> GetByToken(string token)
        {
            LLogin login = LLogin.CheckToken(token);

            List<DOrder> orders = DOrder.GetByCustomerNo(login.CustomerNo);
            return orders != null ? Translate(orders) : null;
        }

        public static LOrder GetByNo(string token, int type, string no)
        {
            LLogin login = LLogin.CheckToken(token);
            //DCustomer customer = DCustomer.GetById(login.CustomerNo);

            LOrder order = GetByNo(type, no);

            if (order.CustomerNo != login.CustomerNo)
                return null;

            return order;
        }

        internal static LOrder GetByNo(int type, string no)
        {
            DOrder dOrder = DOrder.GetByNo(type, no);
            if (dOrder == null)
                return null;

            return new LOrder(dOrder);
        }

        /// <summary>
        /// Изменяем статус заказа
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        public Guid ChangeStatus(string token, int newStatus)
        {
            if(newStatus > 1)
                throw new Exception("Пока можно менять только на подтвержден и обратно.");

            LOrderStatus orderStatus = new LOrderStatus() {DocumentType = DocumentType,DocumentNo = DocumentNo, NewStatus = (OrderStatus)newStatus };
            return orderStatus.Insert(token);
        }

        #endregion
    }
}
