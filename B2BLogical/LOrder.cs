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

        #endregion
    }
}
