#### 背景
面对ORM工具的选型，用过[PetaPoco](https://github.com/CollaboratingPlatypus/PetaPoco)、
[SqlSugar](https://github.com/sunkaixuan/SqlSugar)、[nhibernate-core](https://github.com/nhibernate/nhibernate-core)、[EntityFrameworkCore](https://github.com/aspnet/EntityFrameworkCore)、 ORM选择是一个非常纠结切难以下手的问题。当时围绕Dapper、EF、Nhibernate发生了激烈的讨论。 选择EF因为微软的支持，同时选择Dapper代替ADO.NET 操作数据库，而且Dapper比PetaPoco、SqlSugar好在使用的人多且环境多，出现的问题和支持可以得到很好的解决。

#### 为什么选择EF+Dapper
目前来说EF和Dapper是.NET平台最主流的ORM工具，团队成员的接受程度很高，学习成本最低，因为主主流，所以相关的资料非常齐全，各种坑也最少。

#### 介绍
1. 支持工作单元模式，也支持事务，也支持数据库仓储和工作单元模型 
2. 能帮助你快速的构建项目的数据访问层
3. 支持EF和Dapper  ,简单操作使用EF，复杂sql操作使用Dapper
4. 支持Mysql和Mssql
5. 支持同步和异步操作，推荐使用异步
PS：参考其他开源项目，切使用MIT协议的项目

#### 使用方法
引入nuget
```
<PackageReference Include="Sop.Data" Version="1.0.0" />
```
创建实体对象，继承IEntity

```
    public class School : IEntity
    {
        public int Id { get; set; }
        public string SchoolId { get; set; }
        public string Name { get; set; }
    }
```
创建仓储接口和实现类，分别继承IRepository和EfCoreRepository
```
    public class SchoolRepository: EfCoreRepository<School>,ISchoolRepository
    {
        public SchoolRepository(DbContext context) : base(context)
        {
        }
    }

    public interface ISchoolRepository : IRepository<School>
    {

    }
```
 
注入服务
```
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchoolRepository _schoolRepository; 
    /// <summary>
    /// 
    /// </summary>
    public MsEfDbRepositoryTest()
    {
        var services = new ServiceCollection();
        services.AddSopData<EfDbBaseDbContext>(opt =>
        {
            opt.UseMySql("server =127.0.0.1;database=soptestdb;uid=root;password=123456;");
        });
        var sp = services.BuildServiceProvider();
        _unitOfWork = sp.GetRequiredService<IUnitOfWork>();
        _schoolRepository = sp.GetRequiredService<ISchoolRepository>(); 
            
    }
```
 
在Controller中使用
```
public class ValuesController : ControllerBase
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ValuesController(ISchoolRepository schoolRepository, IUnitOfWork unitOfWork)
    {
        _schoolRepository = schoolRepository;
        _unitOfWork = unitOfWork;
    }
}
```
#### 详细使用说明
查询
```
//根据主键查询
_schoolRepository.GetById(Id)
```
```
//不带追踪的查询，返回数据不能用于更新或删除操作，性能快
schoolRepository.TableNoTracking.First(x => x.Id == Id);
```
```
//带追踪的查询，返回数据可以用于更新或删除操作，性能稍慢
schoolRepository.Table.First(x => x.Id == Id);
```
```
//分页查询
_schoolRepository.TableNoTracking.ToPagedList(1,10);
```
```
//sql语句查询
_unitOfWork.QueryAsync<School>("select * from school");

```
```
//sql分页查询
_unitOfWork.QueryPagedListAsync<School>(1, 10, "select * from school order by id");
```
> 关于查询，暴露了返回IQueryable的TableNoTracking、Table这两个属性，让开发人员自己组装Lambda表达式进行查询操作

新增
```
//新增，支持批量新增
_schoolRepository.Insert(school);
_unitOfWork.SaveChanges();
```
```
//sql语句新增
_unitOfWork.ExecuteAsync("insert school(id,name) values(@Id,@Name)",
                        school);
```
编辑
```
//编辑，支持批量编辑
var school = _schoolRepository.GetByIdAsync(Id);
school.Name="newschool";
_schoolRepository.Update(school);
_unitOfWork.SaveChanges();
```
```
//编辑，不用先查询
var school = new School
{
    Id = "xxxxxx",
    Name = "newschool"
};
_schoolRepository.Update(school, x => x.Name);
_unitOfWork.SaveChanges();
```
```
//sql语句编辑
_unitOfWork.ExecuteAsync("update school set name=@Name where id=@Id",
                        school);
```
删除
```
//删除，支持批量删除
_schoolRepository.Delete(school);
_unitOfWork.SaveChanges();
```
```
//根据lambda删除
_schoolRepository.Delete(x => x.Id == Id);
_unitOfWork.SaveChanges();
```
事务
```
//工作单元模式使用事务
await _schoolRepository.InsertAsync(school1);
await _schoolRepository.InsertAsync(school1);
await _unitOfWork.SaveChangesAsync();
```
```
//dapper使用事务
using (var tran = _unitOfWork.BeginTransaction())
{
    try
    {
        await _unitOfWork.ExecuteAsync("insert school(id,name) values(@Id,@Name)",
            school1,tran);
        await _unitOfWork.ExecuteAsync("insert school(id,name) values(@Id,@Name)",
            school2,tran);
        tran.Commit();
    }
    catch (Exception e)
    {
        tran.Rollback();
    }
}
```
```
//dapper+ef混合使用事务
using (var tran = _unitOfWork.BeginTransaction())
{
    try
    {
        await _schoolRepository.InsertAsync(school1);
        await _unitOfWork.SaveChangesAsync();

        await _unitOfWork.ExecuteAsync("insert school(id,name) values(@Id,@Name)",
            school2);
        tran.Commit();
    }
    catch (Exception e)
    {
        tran.Rollback();
    }
}
```
高级用法
```
//通过GetConnection可以使用更多dapper扩展的方法
_unitOfWork.GetConnection().QueryAsync("select * from school");
```
#### 协议
MIT
 