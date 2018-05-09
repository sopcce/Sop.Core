using System;
using System.Collections.Generic;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

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



    public string Title { get; set; }

    public string Content { get; set; }
    public string HtmlContentPath { get; set; }

    public int ViewCount { get; set; }

    public int DisplayOrder { get; set; }
    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    public DateTime DateCreated { get; set; } 



 
 
  

  }





}
