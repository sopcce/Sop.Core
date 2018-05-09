using System.Text;
using System.Web.Mvc;
using ItemDoc.Core.Mvc.Json;
using Sop.Common.Serialization.Json;

namespace ItemDoc.Web.Controllers.Base
{
  public class BaseController : Controller
  {
    private PropertyNameType _propertyNameType = PropertyNameType.Default;

    public PropertyNameType GetPropertyNameType()
    {
      return _propertyNameType;
    }




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




  }
}