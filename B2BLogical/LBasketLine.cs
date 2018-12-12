using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public class LBasketLine
    {
        #region Properties
        public int BasketNo { get; set; }
        public int LineNo { get; set; }
        public string ItemNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        #endregion

        #region Constructors

        public LBasketLine()
        { }

        internal LBasketLine(B2BData.DBasketLine item)
        {
            Translate(item);
        }

        #endregion

        #region Methods

        internal void Translate(DBasketLine item)
        {
            BasketNo = item.BasketNo;
            LineNo = item.LineNo;
            ItemNo = item.ItemNo;
            Quantity = item.Quantity;
            Price = item.Price;
        }

        public DBasketLine ToData()
        {
            DBasketLine dBasketLine = new DBasketLine() { BasketNo = BasketNo, ItemNo = ItemNo, LineNo = LineNo, Price = Price, Quantity = Quantity };

            return dBasketLine;
        }

        public void Insert()
        {
            ToData().Insert();
        }
        #endregion
    }
}
