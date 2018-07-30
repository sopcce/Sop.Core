using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public class UsersRepository : PocoRepository<UsersInfo>, IUsersRepository
  {
    public UsersInfo GetByUserName(string uName)
    {

      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<UsersInfo>())
        .From(SopTable.Instance().GetTableName<UsersInfo>())
        .Where("UserName=@0", uName);
      return Database().SingleOrDefault<UsersInfo>(sql); 
    }
    
    public UsersInfo GetByUserId(string uId)
    {

      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<UsersInfo>())
        .From(SopTable.Instance().GetTableName<UsersInfo>())
        .Where("UserId=@0", uId);
      return Database().SingleOrDefault<UsersInfo>(sql);
    }

  }
}
