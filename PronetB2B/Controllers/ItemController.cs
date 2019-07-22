using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using B2BLogical;
using System.IO;

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

        // http://png.pronetgroup.ru:116/api/Item/GetSectionsWithItemId
        // http://localhost:60088/api/Item/GetSectionsWithItemId
        public List<LSectionItemId> GetSectionsWithItemId()
        {
            return B2BLogical.LSectionItemId.GetAll();
        }


        //http://localhost:60088/api/Item/GetItemsDecorate?token=TEST_B2B_PRONET
        public List<LItemDecorate> GetItemsDecorate(string token)
        {
            return B2BLogical.LItemDecorate.GetAllByToken(token);
        }

        //http://localhost:60088/api/Item/GetItems?token=TEST_B2B_PRONET
        public List<LItem> GetItems(string token)
        {
            return B2BLogical.LItem.GetAllByToken(token);
        }

        //localhost:60088/api/Item/GetItems?token=TEST_B2B_PRONET&listId=86772,86771
        public List<LItem> GetItems(string token, string listId)
        {
            if (string.IsNullOrEmpty(listId))
                throw new Exception("Список не может быть пустым");
            return B2BLogical.LItem.GetByListId(token, listId);
        }

        //http://localhost:60088/api/Item/GetItem?token=TEST_B2B_PRONET&id=86771
        public LItem GetItem(string token, string id)
        {
            return B2BLogical.LItem.GetByTokenId(token, id);
        }

        //http://localhost:60088/api/Item/GetSectionItems?token=TEST_B2B_PRONET&section=9161802
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

        //https://www.c-sharpcorner.com/article/sending-files-from-web-api/
        //http://localhost:60088/api/Item/GetPicture?code=10738
        [HttpGet]
        public HttpResponseMessage GetPicture(int code)
        {
            LItemPicture picture = LItemPicture.GetByCode(code);
            FileInfo fileInf = new FileInfo(picture.FileName);
            //converting Pdf file into bytes array  
            var dataBytes = File.ReadAllBytes(picture.FileName);
            //adding bytes to memory stream   
            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileInf.Name;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

        /// <summary>
        /// Возвращает все цены по клиенту.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="currency">Валюта. Одно из значений: RUR, USD, "пусто". При значении пусто выдается рублевая цена.</param>
        /// <returns></returns>
        //http://localhost:60088/api/Item/GetAllPrices?token=TEST_B2B_PRONET&currency=USD
        public List<LItemPrice> GetAllPrices(string token, string currency = "")
        {
            return B2BLogical.LItemPrice.GetAllByToken(token);
        }

        /// <summary>
        /// Возвращает цены по товару для клиента.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="itemNo">Код товара</param>
        /// <param name="currency">Валюта. Одно из значений: RUR, USD, "пусто". При значении пусто выдается рублевая цена.</param>
        /// <returns></returns>
        //http://localhost:60088/api/Item/GetPrice?token=TEST_B2B_PRONET&itemNo=505101&currency=RUR
        public decimal GetPrice(string token, string itemNo, string currency = "")
        {
            return B2BLogical.LItemPrice.GetPriceByTokenIdCurr(token, itemNo, currency);
        }

        /// <summary>
        /// Для списка товаров возвращает список цен для данного клиента.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="listId"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        //http://localhost:60088/api/Item/GetPrices?token=TEST_B2B_PRONET&listid=505101,516644,506829&currency=RUR
        public List<LItemPrice> GetPrices(string token, string listId, string currency = "")
        {
            return B2BLogical.LItemPrice.GetByListId(token, listId, currency);
        }

    }
}
