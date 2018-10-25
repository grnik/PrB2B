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
    public class LSection
    {
        #region Properties

        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int LowestLevel { get; set; }
        [DataMember]
        public string Parent { get; set; }
        [DataMember]
        public bool Popular { get; set; }

        #endregion

        #region Constructores

        internal LSection(B2BData.DSection section)
        {
            Translate(section);
        }

        #endregion

        #region Methords

        internal void Translate(B2BData.DSection section)
        {
            Code = section.Code;
            Description = section.Description;
            LowestLevel = section.LowestLevel;
            Parent = section.Parent;
            Popular = section.Popular;
        }

        internal static List<LSection> Translate(List<DSection> dSections)
        {
            List<LSection> res = new List<LSection>();
            foreach (DSection dSection in dSections)
            {
                res.Add(new LSection(dSection));
            }

            return res;
        }

        public static List<LSection> GetAll()
        {
            return Translate(DSection.GetAll());
        }

        #endregion
    }
}
