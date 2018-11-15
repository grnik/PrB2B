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

        //http://localhost:60088/api/Item/GetItems?token=TEST_B2B_PRONET
        public List<LItem> GetItems(string token)
        {
            return B2BLogical.LItem.GetAllByToken(token);
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

    }
}
