using Sop.Common.Serialization;
using Sop.Common.Serialization.Json;
using System;
using System.Web.Mvc;

namespace ItemDoc.Core.Mvc.Json
{
    public class PropertyNameJsonResult : JsonResult
  {
    /// <summary>
    /// 目标对象
    /// </summary>
    public new object Data { get; set; }

    public PropertyNameType PropertyNameType { get; set; } = PropertyNameType.Default;

    /// <summary>
    /// 构造器
    /// </summary>
    public PropertyNameJsonResult() { }

    /// <summary>
    /// 构造器
    /// </summary>
    public PropertyNameJsonResult(object data)
    {
      this.Data = data;
    }
    public PropertyNameJsonResult(PropertyNameType propertyNameType)
    {
      this.PropertyNameType = propertyNameType;
    }
    public PropertyNameJsonResult(object data, PropertyNameType propertyNameType)
    {
      this.Data = data;
      this.PropertyNameType = propertyNameType;
    }



    /// <summary>
    /// 重写执行结果
    /// </summary>
    /// <param name="context"></param>
    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }
      var response = context.HttpContext.Response;
      response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
      if (ContentEncoding != null)
      {
        response.ContentEncoding = ContentEncoding;
      }

      response.Write(this.Data.ToJson(PropertyNameType));
    }


  }
}
