using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public class LBasket
    {
        #region Properties
        public int EntryNo { get; private set; }
        private string CompanyName { get; set; }
        public string CustomerId { get; private set; }
        public string Comment { get; set; }
        public string WantCheaper { get; set; }
        public decimal DeliveryPrice { get; set; }
        public int DeliveryDays { get; set; }
        public string DeliveryServiceCode { get; set; }
        /// <summary>
        /// Время создания
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Если заказ перенесен в систему, то True
        /// </summary>
        public bool Processed { get; private set; }
        /// <summary>
        /// Начали перенос. Если начали и не завершили. Счетчик покажет число раз.
        /// </summary>
        public int Processing { get; private set; }
        /// <summary>
        /// Когда начали перенос (для повторной попытки)
        /// </summary>
        public DateTime ProcessingTime { get; private set; }
        /// <summary>
        /// Номер созданного заказа
        /// </summary>
        public string OrderId { get; private set; }
        /// <summary>
        /// Строки заказа
        /// </summary>
        public List<LBasketLine> Line { get; set; }

        #endregion

        #region Constructors

        public LBasket()
        {
            CompanyName = "RUR";
        }

        internal LBasket(B2BData.DBasket basket)
        {
            Translate(basket);
        }

        #endregion

        #region Methods

        internal void Translate(B2BData.DBasket basket)
        {
            EntryNo = basket.EntryNo;
            CompanyName = basket.CompanyName;
            CustomerId = basket.CustomerId;
            Comment = basket.Comment;
            WantCheaper = basket.WantCheaper;
            DeliveryPrice = basket.DeliveryPrice;
            DeliveryDays = basket.DeliveryDays;
            DeliveryServiceCode = basket.DeliveryServiceCode;
            CreateTime = basket.CreateTime;
            Processed = basket.Processed;
            Processing = basket.Processing;
            ProcessingTime = basket.ProcessingTime;
            OrderId = basket.OrderId;

            List<DBasketLine> lines = DBasketLine.GetByBasket(basket.EntryNo);
            foreach (DBasketLine line in lines)
            {
                Line.Add(new LBasketLine(line));
            }
        }

        public DBasket ToData()
        {
            DBasket basket = new DBasket();

            //basket.EntryNo = EntryNo;
            basket.CompanyName = CompanyName;
            basket.CustomerId = CustomerId;
            basket.Comment = Comment;
            basket.WantCheaper = WantCheaper;

            basket.DeliveryPrice = DeliveryPrice;
            basket.DeliveryDays = DeliveryDays;
            basket.DeliveryServiceCode = DeliveryServiceCode;
            basket.CreateTime = CreateTime;
            basket.Processed = Processed;
            basket.Processing = Processing;
            basket.ProcessingTime = ProcessingTime;
            basket.OrderId = OrderId;

            return basket;
        }

        public int Insert(string token)
        {
            LLogin login = LLogin.CheckToken(token);
            CustomerId = login.CustomerNo;

            CreateTime = DateTime.Now;
            ProcessingTime = CreateTime;//Т.к. в навижене не 0
            int res = ToData().Insert();

            foreach (LBasketLine line in Line)
            {
                line.Insert();
            }

            return res;
        }

        public static LBasket GetByNo(string token, int no)
        {
            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            DBasket dBasket = DBasket.GetByNo(no);
            if (dBasket == null)
                return null;

            if (dBasket.CustomerId != customer.Id)
                return null;

            return new LBasket(dBasket);
        }

        #endregion
    }
}
