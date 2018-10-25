using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using B2BLogical;

namespace PronetB2B.Controllers
{
    public class ItemController : ApiController
    {
        // http://png.pronetgroup.ru:116/api/
        // http://localhost:60088/api/Item/GetSections
        public List<LSection> GetSections()
        {
            return B2BLogical.LSection.GetAll();
        }

        //http://localhost:60088/api/Item/GetItems?token=04190bd9-bc53-45b6-b8ad-8c491a4ccba2
        public List<LItem> GetItems(string token)
        {
            return B2BLogical.LItem.GetAllByToken(token);
        }

        //http://localhost:60088/api/Item/GetSectionItemsItems?token=04190bd9-bc53-45b6-b8ad-8c491a4ccba2&section=9161802
        public List<LItem> GetSectionItems(string token, string section)
        {
            return B2BLogical.LItem.GetAllByTokenParent(token, section);
        }

        //http://localhost:60088/api/Item/GetProperties?itemNo=505101
        public List<LItemProperty> GetProperties(string itemNo)
        {
            return B2BLogical.LItemProperty.GetByItemNo(itemNo);
        }

        //http://localhost:60088/api/Item/GetPictures?itemNo=505101
        public List<LItemPicture> GetPictures(string itemNo)
        {
            return B2BLogical.LItemPicture.GetByItemNo(itemNo);
        }
    }
}
