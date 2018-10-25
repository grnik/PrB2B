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
    public class LItemProperty
    {
        #region Properties

        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public string ItemNo { get; set; }
        [DataMember]
        public string GroupPropertyCode { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public int Order { get; set; }

        #endregion Properties

        #region Constructores

        internal LItemProperty(B2BData.DItemProperty essential)
        {
            Translate(essential);
        }

        #endregion

        #region Methords

        internal void Translate(DItemProperty property)
        {
            Code = property.Code;
            Description = property.Description;
            Type = property.Type;
            ItemNo = property.ItemNo;
            GroupPropertyCode = property.GroupPropertyCode;
            Value = property.Value;
            Order = property.Order;
        }

        internal static List<LItemProperty> Translate(List<DItemProperty> dItemPropertys)
        {
            List<LItemProperty> res = new List<LItemProperty>();
            foreach (DItemProperty dItemProperty in dItemPropertys)
            {
                res.Add(new LItemProperty(dItemProperty));
            }

            return res;
        }

        public static List<LItemProperty> GetByItemNo(string itemNo)
        {
            List<DItemProperty> properties = DItemProperty.GetByItemNo(itemNo);
            return properties != null ? Translate(properties) : null;
        }

        #endregion
    }
}
