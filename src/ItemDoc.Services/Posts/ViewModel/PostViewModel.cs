using System;
using System.Collections.Generic;
using ItemDoc.Core.Mvc.Json;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;
using Newtonsoft.Json;
using Sop.Common.Serialization.Json;

namespace ItemDoc.Services.ViewModel
{
  /// <summary>
  /// 用户登录实体
  /// </summary>

  public class PostViewModel
  {


    public int Id { get; set; }

    public int CatalogId { get; set; }


    public string UserId { get; set; }

    public string NickName { get; set; }

    public string Description { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
    public string HtmlContentPath { get; set; }

    public int ViewCount { get; set; }

    public int DisplayOrder { get; set; }
    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    //[JsonConverter(typeof(JsonTimeStampConverter))]
    public DateTime DateCreated { get; set; }


    public DateTime DateCreatedTime { get; set; } = DateTime.Now;

    public string CreatedIP { get; set; }



  }





}
