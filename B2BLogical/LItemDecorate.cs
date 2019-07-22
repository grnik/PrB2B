using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace B2BLogical
{
    [DataContract]
    public class LItemDecorate
    {
        #region Properties
        [DataMember]
        public LItem Item { get; private set; }

        [DataMember]
        public List<LItemProperty> Properties
        {
            get { return LItemProperty.GetByItemNo(Item.Id); }
        }

        [DataMember]
        public List<LItemPicture> Pictures
        {
            get { return LItemPicture.GetByItemNo(Item.Id); }
        }
        #endregion

        #region Constructors
        public LItemDecorate(LItem item)
        {
            Item = item;
        }

        #endregion

        #region Methods

        public static List<LItemDecorate> GetAllByToken(string token)
        {
            List<LItemDecorate> res = new List<LItemDecorate>();

            foreach (LItem item in LItem.GetAllByToken(token))
            {
                res.Add(new LItemDecorate(item));
            }

            return res;
        }

        #endregion
    }
}
