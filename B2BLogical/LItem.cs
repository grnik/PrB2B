using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public struct LItemAvailable
    {
        public string Id;
        public decimal Available;

        internal LItemAvailable(DItemAvailable itemAvailable)
        {
            Id = itemAvailable.Id;
            Available = itemAvailable.Available;
        }

        public override string ToString()
        {
            return Id+"=>"+Available.ToString(CultureInfo.CurrentCulture);
        }

        public static string ToString(List<LItemAvailable> list)
        {
            StringBuilder res = new StringBuilder();
            foreach (LItemAvailable itemAvailable in list)
            {
                if (res.Length != 0)
                {
                    res.Append(",");
                }

                res.Append(itemAvailable.Id + "=>" + itemAvailable.Available.ToString(CultureInfo.CurrentCulture));
            }

            return res.ToString();
        }
    }

    [DataContract]
    public class LItem
    {
        #region Properties

        private static List<LCurrencyExchangeRate> _currencyCash = null;

        [DataMember] public string Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string NameRus { get; set; }
        [DataMember] public string PartNumber { get; set; }
        [DataMember] public string Model { get; set; }
        [DataMember] public string EAN { get; set; }
        [DataMember] public bool Popular { get; set; }
        [DataMember] public bool Profitable { get; set; }
        [DataMember] public bool NewIncome { get; set; }
        [DataMember] public string ParentCode { get; set; }
        [DataMember] public decimal Available { get; set; }
        [DataMember] public decimal Weight { get; set; }
        [DataMember] public string BalanceType { get; set; }
        [DataMember] public decimal Volume { get; set; }
        [DataMember] public string Guarantee { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public decimal PriceRUR { get; set; }
        [DataMember]
        public string PriceCurrency { get; set; }
        [DataMember]
        public string PriceType { get; set; }

        #endregion Properties

        #region Constructores

        internal LItem(B2BData.DItem item)
        {
            Translate(item);
        }

        #endregion

        #region Methords

        internal void Translate(DItem item)
        {

            Id = item.Id;
            Name = item.Name;
            NameRus = item.NameRus;
            PartNumber = item.PartNumber;
            Model = item.Model;
            EAN = item.EAN;
            Popular = item.Popular;
            Profitable = item.Profitable;
            NewIncome = item.NewIncome;
            ParentCode = item.ParentCode;
            Available = item.Available;
            Weight = item.Weight;
            BalanceType = item.BalanceType;
            Volume = item.Volume;
            Price = Math.Round(item.Price, 2);
            PriceCurrency = item.PriceCurrency;
            PriceType = item.PriceType;
            Guarantee = item.Guarantee;

            PriceRUR = Math.Round(Price * GetCourse(), 2);
        }

        internal static List<LItem> Translate(List<DItem> dItems)
        {
            _currencyCash = new List<LCurrencyExchangeRate>();

            List<LItem> res = new List<LItem>();
            foreach (DItem dItem in dItems)
            {
                res.Add(new LItem(dItem));
            }

            return res;
        }

        public static LItem GetByTokenId(string token, string id)
        {
            _currencyCash = new List<LCurrencyExchangeRate>();

            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            return GetByPriceTypeId(customer, id);
        }

        public static decimal GetAvailableByTokenId(string token, string id)
        {
            LLogin.CheckToken(token);

            return DItem.GetAvailableById(id);
        }

        /// <summary>
        /// Возвращаем список доступного товара и его кол-во
        /// </summary>
        /// <returns></returns>
        public static List<LItemAvailable> GetAvailablesByToken(string token)
        {
            LLogin.CheckToken(token);

            List<DItemAvailable> availables = DItem.GetAvailables();
            List<LItemAvailable> result = new List<LItemAvailable>();
            foreach (DItemAvailable dItemAvailable in availables)
            {
                result.Add(new LItemAvailable(dItemAvailable));
            }

            return result;
        }

        internal static LItem GetByPriceTypeId(DCustomer customer, string id)
        {
            DItem item = DItem.GetByPriceTypeId(customer.PriceType, id);
            return item != null ? new LItem(item) : null;
        }

        public static List<LItem> GetByListId(string token, string listId)
        {
            _currencyCash = new List<LCurrencyExchangeRate>();

            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            string[] ids = listId.Split(',');
            List<LItem> items = new List<LItem>();
            foreach (string id in ids)
            {
                items.Add(LItem.GetByPriceTypeId(customer, id));
            }

            return items;
        }

        public static List<LItem> GetAllByToken(string token)
        {
            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            List<DItem> items = DItem.GetAllByPriceType(customer.PriceType);
            return items != null ? Translate(items) : null;
        }

        public static List<LItem> GetAllByTokenParent(string token, string parent, bool withChildren = true)
        {
            LLogin login = LLogin.CheckToken(token);
            DCustomer customer = DCustomer.GetById(login.CustomerNo);

            List<DItem> items = DItem.GetByParent(customer.PriceType, parent, withChildren);
            return items != null ? Translate(items) : null;
        }

        /// <summary>
        /// Возвращаем курс валюты цены.
        /// </summary>
        /// <returns></returns>
        private decimal GetCourse()
        {
            if (String.IsNullOrEmpty(PriceCurrency))
                return 1;

            LCurrencyExchangeRate curr = _currencyCash.FirstOrDefault(c => c.CurrencyCode == PriceCurrency);
            if (curr == null)
            {
                curr = LCurrencyExchangeRate.GetByLastCurrencyRate(PriceCurrency);
                _currencyCash.Add(curr);
            }
            return curr.ExchangeRate;
        }

        #endregion
    }
}
