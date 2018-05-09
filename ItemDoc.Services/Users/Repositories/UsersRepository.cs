using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public class UsersRepository : PocoRepository<UsersLoginInfo>, IUsersRepository
  {
    public UsersLoginInfo GetByUserName(string uName)
    {

      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<UsersLoginInfo>())
        .From(SopTable.Instance().GetTableName<UsersLoginInfo>())
        .Where("UserName=@0", uName);
      return Database().SingleOrDefault<UsersLoginInfo>(sql); 
    }
    
    public UsersLoginInfo GetByUserId(string uId)
    {

      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<UsersLoginInfo>())
        .From(SopTable.Instance().GetTableName<UsersLoginInfo>())
        .Where("UserId=@0", uId);
      return Database().SingleOrDefault<UsersLoginInfo>(sql);
    }

  }
}
