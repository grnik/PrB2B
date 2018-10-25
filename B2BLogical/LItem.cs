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
    public class LItem
    {
        #region Properties

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
        [DataMember] public decimal Price { get; set; }
        [DataMember] public string PriceCurrency { get; set; }
        [DataMember] public string PriceType { get; set; }
        [DataMember] public string Guarantee { get; set; }

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
            Price = item.Price;
            PriceCurrency = item.PriceCurrency;
            PriceType = item.PriceType;
            Guarantee = item.Guarantee;
        }

        internal static List<LItem> Translate(List<DItem> dItems)
        {
            List<LItem> res = new List<LItem>();
            foreach (DItem dItem in dItems)
            {
                res.Add(new LItem(dItem));
            }

            return res;
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

        #endregion
    }
}
