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
    public class LBasket
    {
        #region Properties
        /// <summary>
        /// Номер записи. При создании не указывать
        /// </summary>
        [DataMember]
        public int EntryNo { get; private set; }
        /// <summary>
        /// Код нашей компании
        /// </summary>
        [DataMember]
        private string CompanyName { get; set; }
        /// <summary>
        /// Код клиента
        /// </summary>
        [DataMember]
        public string CustomerId { get; private set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
        /// <summary>
        /// Клиент хочет дешевле
        /// </summary>
        [DataMember]
        public string WantCheaper { get; set; }
        /// <summary>
        /// Стоимость доставки
        /// </summary>
        [DataMember]
        public decimal DeliveryPrice { get; set; }
        /// <summary>
        /// Срок доставки
        /// </summary>
        [DataMember]
        public int DeliveryDays { get; set; }
        /// <summary>
        /// Код агента по доставке
        /// </summary>
        [DataMember]
        public string DeliveryServiceCode { get; set; }
        /// <summary>
        /// Дата отгрузки
        /// </summary>
        [DataMember]
        public DateTime? ShippingDate { get; set; }
        /// <summary>
        /// Реквизиты клиента
        /// </summary>
        [DataMember]
        public string EssentialCode { get; set; }
        /// <summary>
        /// Время создания
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Если заказ перенесен в систему, то True
        /// </summary>
        [DataMember]
        public bool Processed { get; private set; }
        /// <summary>
        /// Начали перенос. Если начали и не завершили. Счетчик покажет число раз.
        /// </summary>
        [DataMember]
        public int Processing { get; private set; }
        /// <summary>
        /// Когда начали перенос (для повторной попытки)
        /// </summary>
        [DataMember]
        public DateTime ProcessingTime { get; private set; }
        /// <summary>
        /// Номер созданного заказа
        /// </summary>
        [DataMember]
        public string OrderId { get; private set; }
        /// <summary>
        /// Строки заказа
        /// </summary>
        [DataMember]
        public List<LBasketLine> Line { get; set; }
        /// <summary>
        /// Оплата в кредит.
        /// </summary>
        [DataMember]
        public decimal InCredit { get; set; }
        /// <summary>
        /// Счет на сумму. На какую сумму клиенту высталвен счет на данный заказ.
        /// </summary>
        [DataMember]
        public decimal AccountSum { get; set; }
        [DataMember]
        public string ShippingNote { get; set; }
        /// <summary>
        /// Время отправки уведомления
        /// </summary>
        public DateTime? Notice { get; set; }

        #endregion

        #region Constructors

        public LBasket()
        {
            CompanyName = "RUR";
            Line = new List<LBasketLine>();
        }

        internal LBasket(B2BData.DBasket basket)
        {
            Line = new List<LBasketLine>();
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
            ShippingDate = basket.ShippingDate;
            EssentialCode = basket.EssentialCode;
            CreateTime = basket.CreateTime;
            Processed = basket.Processed;
            Processing = basket.Processing;
            ProcessingTime = basket.ProcessingTime;
            OrderId = basket.OrderId;
            InCredit = basket.InCredit;
            AccountSum = basket.AccountSum;
            ShippingNote = basket.ShippingNote;
            Notice = basket.Notice;

            List<DBasketLine> lines = DBasketLine.GetByBasket(basket.EntryNo);
            foreach (DBasketLine line in lines)
            {
                Line.Add(new LBasketLine(line));
            }
        }


        internal static List<LBasket> Translate(List<DBasket> dBaskets)
        {
            List<LBasket> res = new List<LBasket>();
            foreach (DBasket dBasket in dBaskets)
            {
                res.Add(new LBasket(dBasket));
            }

            return res;
        }

        public static explicit operator DBasket(LBasket lBasket)
        {
            DBasket basket = new DBasket();

            basket.EntryNo = lBasket.EntryNo;
            basket.CompanyName = lBasket.CompanyName;
            basket.CustomerId = lBasket.CustomerId;
            basket.Comment = lBasket.Comment;
            basket.WantCheaper = lBasket.WantCheaper;

            basket.DeliveryPrice = lBasket.DeliveryPrice;
            basket.DeliveryDays = lBasket.DeliveryDays;
            basket.DeliveryServiceCode = lBasket.DeliveryServiceCode;
            basket.ShippingDate = lBasket.ShippingDate.HasValue ? basket.ShippingDate = lBasket.ShippingDate.Value : LConst.MinDateNavision;
            basket.EssentialCode = lBasket.EssentialCode;
            basket.CreateTime = lBasket.CreateTime;
            basket.Processed = lBasket.Processed;
            basket.Processing = lBasket.Processing;
            basket.ProcessingTime = lBasket.ProcessingTime;
            basket.OrderId = lBasket.OrderId;
            basket.InCredit = lBasket.InCredit;
            basket.AccountSum = lBasket.AccountSum;
            basket.ShippingNote = lBasket.ShippingNote;
            basket.Notice = lBasket.Notice.HasValue ? basket.Notice = lBasket.Notice.Value : LConst.MinDateNavision;

            return basket;
        }

        public int Insert(string token)
        {
            LLogin login = LLogin.CheckToken(token);
            CustomerId = login.CustomerNo;

            CreateTime = DateTime.Now;
            ProcessingTime = CreateTime;//Т.к. в навижене не 0
            Processed = false;
            Processing = 0;
            OrderId = string.Empty;

            DBasket dBasket = (DBasket)this;
            EntryNo = dBasket.Insert();

            foreach (LBasketLine line in Line)
            {
                line.BasketNo = EntryNo;
                line.Insert();
            }

            return EntryNo;
        }

        /// <summary>
        /// Фиксируем факт отправки сообщения по данной корзине.
        /// </summary>
        public void FixNotice()
        {
            Notice = DateTime.Now;
            DBasket dBasket = (DBasket)this;
            
            dBasket.SaveNotice();
        }

        public static LBasket GetByNo(string token, int no)
        {
            LLogin login = LLogin.CheckToken(token);
            //DCustomer customer = DCustomer.GetById(login.CustomerNo);

            DBasket dBasket = DBasket.GetByNo(no);
            if (dBasket == null)
                return null;

            if (dBasket.CustomerId != login.CustomerNo)
                return null;

            return new LBasket(dBasket);
        }

        /// <summary>
        /// Вернуть заявки на создание заказа, по которым нет уведомлений.
        /// </summary>
        /// <returns></returns>
        internal static List<LBasket> WithoutNotice()
        {
            return Translate(DBasket.WithoutNotice());
        }

        #endregion
    }
}
