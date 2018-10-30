using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ItemDoc.Core.API;
using ItemDoc.Core.Mvc.Json;
using Sop.Common.Serialization.Json;

namespace ItemDoc.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        #region MyRegion
        private PropertyNameType _propertyNameType = PropertyNameType.Default;

        public PropertyNameType GetPropertyNameType()
        {
            return _propertyNameType;
        }
        #endregion
        #region  Result

        /// <summary>
        /// 返回逻辑错误
        /// </summary>
        /// <param name="result"></param>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public HttpResponseMessage ErrorResult(object result, HttpStatusCode code = HttpStatusCode.BadRequest, string desc = "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var content = new DataPackage(code, result, desc);

            var response = new HttpResponseMessage { Content = new StringContent(serializer.Serialize(content), Encoding.UTF8, "application/json") };

            return response;
        }
        /// <summary>
        /// 返回成功信息
        /// </summary>
        /// <param name="result"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public HttpResponseMessage SuccessResult(object result, string desc = "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var content = new DataPackage(HttpStatusCode.OK, result, desc);

            var response = new HttpResponseMessage { Content = new StringContent(serializer.Serialize(content), Encoding.UTF8, "application/json") };

            return response;
        }
   

        #endregion private Result
        public void SetPropertyNameType(PropertyNameType value)
        {
            _propertyNameType = value;
        }

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

        public JsonResult JsonSuccessResult(object result, string desc = "")
        {
            this.SetPropertyNameType(PropertyNameType.ToLower);
            return this.Json(new DataPackage(HttpStatusCode.OK, result, desc), null, null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JsonErrorResult(object result, string desc = "")
        {
            this.SetPropertyNameType(PropertyNameType.ToLower);
            return this.Json(new DataPackage(HttpStatusCode.BadRequest, result, desc), null, null, JsonRequestBehavior.AllowGet);
        }

    }
}