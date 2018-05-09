using System;
using ItemDoc.Framework.Repositories;
using ItemDoc.Framework.SystemLog;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Model;
using ItemDoc.Services.ViewModel;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public class PostRepository : PocoRepository<PostInfo>, IPostRepository
  {
    public PostInfo GetById(string primaryKey)
    {
      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<PostInfo>())
        .From(SopTable.Instance().GetTableName<PostInfo>())
        .Where("Id=@0", primaryKey);
      return Database().SingleOrDefault<PostInfo>(sql);
    }

    public IPageList<PostInfo> GetPostList(PostParameter parameter)
    {



      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<PostInfo>())
        .From(SopTable.Instance().GetTableName<PostInfo>());





      return Gets(parameter.pageIndex, parameter.pageSize, sql.SQL, sql.Arguments);



    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public IPageList<PostViewModel> GetPostList1(PostParameter parameter)
    {



      Sql sql = Sql.Builder
        .Select("Item_Posts.*,Item_UsersLogin.NickName,Item_Catalog.Name")
        .From(SopTable.Instance().GetTableName<PostInfo>())
        .LeftJoin(SopTable.Instance().GetTableName<UsersLoginInfo>())
        .On("Item_UsersLogin.UserId=Item_Posts.UserId")
        .LeftJoin(SopTable.Instance().GetTableName<CatalogInfo>())
        .On("Item_Catalog.Id=Item_Posts.CatalogId")
        .Where("CatalogId=@0", parameter.CatalogId);

      Log.Instance().Write(sql.ToString(), null, null, LogLevel.Info);
      switch (parameter.sortName)
      {
        case SortName.Title:
          sql = parameter.sortOrder == SortOrder.Asc
            ? sql.OrderBy(" Item_Posts.Title ASC")
            : sql.OrderBy(" Item_Posts.Title DESC");
          break;
        case SortName.DisplayOrder:

          sql = parameter.sortOrder == SortOrder.Asc
            ? sql.OrderBy(" Item_Posts.DisplayOrder ASC")
            : sql.OrderBy(" Item_Posts.DisplayOrder DESC");
          break;
        case SortName.DateCreated:
          sql = parameter.sortOrder == SortOrder.Asc
            ? sql.OrderBy(" Item_Posts.DateCreated ASC")
            : sql.OrderBy(" Item_Posts.DateCreated DESC");
          break;
        case SortName.ViewCount:
          sql = parameter.sortOrder == SortOrder.Asc
            ? sql.OrderBy(" Item_Posts.ViewCount ASC")
            : sql.OrderBy(" Item_Posts.ViewCount DESC");
          break;

      }
      var list = Gets<PostViewModel>(parameter.pageIndex, parameter.pageSize, sql.SQL, sql.Arguments);
      return list;
    }
  }
}
