using ItemDoc.Core.API;
using ItemDoc.Core.Mvc.Json;
using Sop.Common.Serialization.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ItemDoc.Web.Controllers
{
    public class BaseController : Controller
    {
        #region private
        private PropertyNameType _propertyNameType = PropertyNameType.Default;

        private PropertyNameType GetPropertyNameType()
        {
            return _propertyNameType;
        }
        private void SetPropertyNameType(PropertyNameType value)
        {
            _propertyNameType = value;
        }
        #endregion

        #region Json

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new PropertyNameJsonResult(GetPropertyNameType()) { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };

        }
        public JsonResult Json(object data, PropertyNameType propertyNameType)
        {
            this.SetPropertyNameType(propertyNameType);
            return this.Json(data, null, null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 输出全部小写属性格式的json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public new JsonResult Json(object data)
        {
            this.SetPropertyNameType(PropertyNameType.ToLower);
            return this.Json(data, null, null, JsonRequestBehavior.AllowGet);
        }

        #region Json Success Result 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public JsonResult JsonSuccessResult(object result, string msg = "")
        {
            this.SetPropertyNameType(PropertyNameType.ToLower);
            return this.Json(new DataPackage(HttpStatusCode.OK, result, msg), null, null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public HttpResponseMessage JsonSuccessResult(object result, string msg, HttpStatusCode code = HttpStatusCode.OK)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var content = new DataPackage(code, result, msg);

            var response = new HttpResponseMessage { Content = new StringContent(serializer.Serialize(content), Encoding.UTF8, "application/json") };

            return response;
        }

        #endregion


        #region Json Error Result
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public JsonResult JsonErrorResult(object result, string msg = "")
        {
            this.SetPropertyNameType(PropertyNameType.ToLower);
            return this.Json(new DataPackage(HttpStatusCode.BadRequest, result, msg), null, null, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回逻辑错误
        /// </summary>
        /// <param name="result"></param>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public HttpResponseMessage JsonErrorResult(object result, string desc = "", HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var content = new DataPackage(code, result, desc);

            var response = new HttpResponseMessage { Content = new StringContent(serializer.Serialize(content), Encoding.UTF8, "application/json") };

            return response;
        }
        #endregion

        #endregion






    }
}