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
    public class LItemPrice
    {
        #region Properties

        [DataMember]
        public string ItemId { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public string PriceCurrency { get; set; }
        [IgnoreDataMember]
        public string CustomerNo { get; set; }

        #endregion Properties

        #region Constructores

        internal LItemPrice(B2BData.DItemPrice itemPrice)
        {
            Translate(itemPrice);
        }

        #endregion

        #region Methords

        internal void Translate(DItemPrice itemPrice)
        {

            ItemId = itemPrice.ItemNo;
            Price = Math.Round(itemPrice.Price, 2);
            PriceCurrency = itemPrice.Currency;
            CustomerNo = itemPrice.CustomerNo;
        }

        internal static List<LItemPrice> Translate(List<DItemPrice> dItems)
        {
            List<LItemPrice> res = new List<LItemPrice>();
            foreach (DItemPrice dItem in dItems)
            {
                res.Add(new LItemPrice(dItem));
            }

            return res;
        }

        public static List<LItemPrice> GetAllByToken(string token, string currency = "")
        {
            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            List<DItemPrice> items = DItemPrice.GetAllByCustCurr(customer.Id, CheckCurrency(currency));
            return items != null ? Translate(items) : null;
        }

        public static LItemPrice GetByTokenId(string token, string id, string currency = "")
        {
            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            return GetByCustItemCurr(customer, id, currency);
        }

        internal static LItemPrice GetByCustItemCurr(DCustomer customer, string id, string currency)
        {
            DItemPrice item = DItemPrice.GetByCustItemCurr(customer.Id, id, CheckCurrency(currency));
            return item != null ? new LItemPrice(item) : null;
        }

        internal static decimal GetPriceByCustItemCurr(DCustomer customer, string id, string currency)
        {
            return DItemPrice.GetPriceByCustItemCurr(customer.Id, id, CheckCurrency(currency));
        }

        public static decimal GetPriceByTokenIdCurr(string token, string id, string currency)
        {
            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            return GetPriceByCustItemCurr(customer, id, currency);
        }


        /// <summary>
        /// Возвращает список цен по переданному списку товаров, для привязанного к токену клиента.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="listId"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static List<decimal> GetPriceByListId(string token, string listId, string currency)
        {
            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            string[] ids = listId.Split(',');
            List<decimal> res = new List<decimal>();
            foreach (string id in ids)
            {
                res.Add(GetPriceByCustItemCurr(customer, id, currency));
            }

            return res;
        }

        /// <summary>
        /// Возвращает список цен по переданному списку товаров, для привязанного к токену клиента.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="listId"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static List<LItemPrice> GetByListId(string token, string listId, string currency)
        {
            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            string[] ids = listId.Split(',');
            List<LItemPrice> res = new List<LItemPrice>();
            foreach (string id in ids)
            {
                res.Add(GetByCustItemCurr(customer, id, currency));
            }

            return res;
        }

        /// <summary>
        /// Проверяем, что валюта указана правильно
        /// </summary>
        /// <param name="currency"></param>
        public static string CheckCurrency(string currency)
        {
            if (string.IsNullOrEmpty(currency))
                return String.Empty;
            if (currency == "USD")
                return "USD";
            if (currency == "RUR")
                return String.Empty;

            throw new Exception("Неизвестный код валюты " + currency);
        }

        #endregion
    }
}
