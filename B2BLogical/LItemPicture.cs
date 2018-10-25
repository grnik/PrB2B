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
    public class LItemPicture
    {
        #region Properties

        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public string ItemNo { get; set; }

        #endregion Properties

        #region Constructores

        internal LItemPicture(B2BData.DItemPicture essential)
        {
            Translate(essential);
        }

        #endregion

        #region Methords

        internal void Translate(DItemPicture property)
        {
            Code = property.Code;
            FileName = property.FileName;
            Type = property.Type;
            ItemNo = property.ItemNo;
        }

        internal static List<LItemPicture> Translate(List<DItemPicture> dItemPictures)
        {
            List<LItemPicture> res = new List<LItemPicture>();
            foreach (DItemPicture dItemPicture in dItemPictures)
            {
                res.Add(new LItemPicture(dItemPicture));
            }

            return res;
        }

        public static List<LItemPicture> GetByItemNo(string itemNo)
        {
            List<DItemPicture> properties = DItemPicture.GetByItemNo(itemNo);
            return properties != null ? Translate(properties) : null;
        }

        #endregion
    }
}
