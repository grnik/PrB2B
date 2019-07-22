using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public enum OrderLineType { NotDef = 0, GlAccount = 1, Item = 2, Resource = 3, FixedAsset = 4, Charge = 5 }
    [DataContract]
    public class LOrderLine
    {
        #region Properties
        public LOrder Order { get; set; }
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

        public LOrderLine()
        { }

        internal LOrderLine(LOrder order, B2BData.DOrderLine item)
        {
            Translate(order, item);
        }

        #endregion

        #region Methods

        internal void Translate(LOrder order, DOrderLine item)
        {
            Order = order;
            LineNo = item.LineNo;
            Type = (OrderLineType)item.Type;
            No = item.No;
            Description = item.Description;
            Quantity = item.Quantity;
            Price = item.Price;
            Comment = item.Comment;
            HedgAmount = item.HedgAmount;
            DopFinance = item.DopFinance;
        }

        public static explicit operator DOrderLine(LOrderLine lOrderLine)
        {
            DOrderLine dOrderLine = new DOrderLine()
            {
                DocumentType = lOrderLine.Order.DocumentType,
                DocumentNo = lOrderLine.Order.DocumentNo,
                LineNo = lOrderLine.LineNo,
                Type = (int)(lOrderLine.Type),
                No = lOrderLine.No,
                Description = lOrderLine.Description,
                Price = lOrderLine.Price,
                Quantity = lOrderLine.Quantity,
                Comment = lOrderLine.Comment,
                HedgAmount = lOrderLine.HedgAmount,
                DopFinance = lOrderLine.DopFinance
            };

            return dOrderLine;
        }
        #endregion
    }
}
