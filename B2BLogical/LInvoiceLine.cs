using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public class LInvoiceLine
    {
        #region Properties
        private LInvoice Invoice { get; set; }
        [DataMember]
        public int LineNo { get; set; }
        [DataMember]
        public OrderLineType Type { get; set; }
        [DataMember]
        public string No { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Quantity { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public decimal HedgAmount { get; set; }
        [DataMember]
        public decimal DopFinance { get; set; }
        #endregion

        #region Constructors

        public LInvoiceLine()
        { }

        internal LInvoiceLine(LInvoice order, B2BData.DInvoiceLine item)
        {
            Translate(order, item);
        }

        #endregion

        #region Methods

        internal void Translate(LInvoice order, DInvoiceLine item)
        {
            Invoice = order;
            LineNo = item.LineNo;
            No = item.No;
            Description = item.Description;
            Quantity = item.Quantity;
            Price = item.Price;
            Comment = item.Comment;
            HedgAmount = item.HedgAmount;
            DopFinance = item.DopFinance;
        }

        public static explicit operator DInvoiceLine(LInvoiceLine lInvoiceLine)
        {
            DInvoiceLine dInvoiceLine = new DInvoiceLine()
            {
                DocumentNo = lInvoiceLine.Invoice.DocumentNo,
                LineNo = lInvoiceLine.LineNo,
                Type = (int)(lInvoiceLine.Type),
                No = lInvoiceLine.No,
                Description = lInvoiceLine.Description,
                Price = lInvoiceLine.Price,
                Quantity = lInvoiceLine.Quantity,
                Comment = lInvoiceLine.Comment,
                HedgAmount = lInvoiceLine.HedgAmount,
                DopFinance = lInvoiceLine.DopFinance
            };

            return dInvoiceLine;
        }
        #endregion
    }
}
