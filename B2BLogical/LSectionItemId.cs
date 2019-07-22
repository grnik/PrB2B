using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    /// <summary>
    /// К описанию секции добавляем список кодов товара.
    /// </summary>
    [DataContract]
    public class LSectionItemId : LSection
    {
        #region Properties

        [DataMember]
        public List<string> ItemId
        {
            get
            {
                List<string> itemsId = DItem.GetItemIdByParent(Code, false, true);
                return itemsId;
            }
        }

        #endregion

        #region Constructors

        internal LSectionItemId(B2BData.DSection section) : base(section)
        {
        }

        #endregion

        #region Methods

        new public static List<LSectionItemId> GetAll()
        {
            List<LSectionItemId> res = new List<LSectionItemId>();
            List<DSection> dSections = DSection.GetAll();
            foreach (DSection dSection in dSections)
            {
                res.Add(new LSectionItemId(dSection));
            }

            return res;
        }

        #endregion


    }
}
