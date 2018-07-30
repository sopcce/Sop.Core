using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using ItemDoc.Framework.Caching;
using ItemDoc.Framework.Repositories;
using ItemDoc.Framework.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemDoc.Framework.Test.Repositories
{
  /// <summary>
  /// 数据访问测试类
  /// </summary>
  [TestClass]
  public class RepositoryTest
  {

    #region 初始化类资源
    private static IRepository<TestInfo> repository;

    /// <summary>
    /// 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
    /// </summary>
    /// <param name="testContext"></param>
    [ClassInitialize()]
    public static void ClassInitialize(TestContext testContext)
    {
      repository = new PocoRepository<TestInfo>();
    }

    // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    //在运行每个测试之前，使用 TestInitialize 来运行代码
    [TestInitialize()]
    public void MyTestInitialize()
    {
      TestUtility.Init();


    }

    // 在每个测试运行完之后，使用 TestCleanup 来运行代码
    [TestCleanup()]
    public void MyTestCleanup()
    {

    }

    private void CleranTable()
    {
      //repository.Execute("truncate table Sop_TestInfo");
    }

    #endregion

    #region GetById_Table

    /// <summary>
    /// 测试Repository<T>的功能
    /// </summary>
    [Description("测试Repository<T>的功能")]
    [TestMethod]
    public void GetById_Table()
    {
      //CleranTable();
      string guid = Guid.NewGuid().ToString();
      TestInfo newEntity = new TestInfo();
      newEntity.Type = TestInfoType.Success;
      newEntity.IsDel = true;
      newEntity.Status = true;
      newEntity.Body = guid;
      newEntity.DateCreated = DateTime.Now;
      newEntity.DecimalValue = 300.5m; ;
      newEntity.FloatValue = 5.4f;
      newEntity.LongValue = 4294967296L;

      SopTable.GetColumnStr(newEntity.IsDel);





      /////添加一条
      for (int i = 0; i < 2; i++)
      {
        //System.Threading.Thread.Sleep(100);
        var info1 = repository.Insert(newEntity);
      }
      var infoPage = repository.Gets(1, 25, null);

      var infopage1 = new PageList<TestInfo>(infoPage, 3, 5);



      Stopwatch sw = new Stopwatch();
      sw.Start();
      var table = repository.Table;

      sw.Stop();
      var data = sw.ElapsedMilliseconds;
      ///1536  /46529 /18361
      Assert.IsNotNull(data);
      Assert.IsNotNull(table.Count());
      //Assert.AreEqual(table.Select(n=>n.Type == TestInfoType.Success).Count(), 1);
      var info = repository.GetById(1);
      //Assert.AreEqual(info.Body, guid);

    }
    #endregion

    #region Insert_Update_Delete
    /// <summary>
    /// 
    /// 测试Repository<T>的功能
    /// </summary>
    [Description("测试Repository<T>的功能")]
    [TestMethod]
    public void Insert_Update_Delete()
    {
      CleranTable();
      List<TestInfo> infos = new List<TestInfo>();
      for (int i = 0; i < 10; i++)
      {
        infos.Add(new TestInfo()
        {
          Body = i + ":Insert",
          Type = TestInfoType.Success,
          IsDel = true,
          Status = true,
          DateCreated = DateTime.Now,
          DecimalValue = 300.5m,
          FloatValue = 5.4f,
          LongValue = i
        });
      }
      //添加10条
      repository.Insert(infos);
      Assert.AreEqual(repository.Table.Count(), 10);
      var infos1 = repository.Table.ToList();
      foreach (var info in infos1)
        info.Body = info.Body + ":Update";
      //修改10条
      repository.Update(infos1);
      Assert.AreEqual(repository.Table.Select(n => n.Body.Contains("Update")).Count(), 10);
      //删除10条
      repository.Delete(infos1);
      Assert.AreEqual(repository.Table.Count(), 0);



    }
    #endregion

    #region Insert&&Table 50000
    /// <summary>
    /// 测试Repository<T> Insert的功能
    /// </summary>
    [Description("测试Repository<T> Insert的功能")]
    [TestMethod]
    public void Insert_Table_50000()
    {
      List<TestInfo> infos = new List<TestInfo>();
      for (int i = 0; i < 100; i++)
      {
        infos.Add(new TestInfo()
        {
          Body = i + ":Insert",
          Type = TestInfoType.Success,
          IsDel = true,
          Status = true,
          DateCreated = DateTime.Now,
          DecimalValue = 300.5m,
          FloatValue = 5.4f,
          LongValue = Convert.ToInt64(i.ToString().PadLeft(10, '1'))
        });
      }
      //添加10条
      repository.Insert(infos);
      var count = repository.Table.Count();
      Assert.AreEqual(count > 100 ? true : false, true);




    }
    #endregion
    /// <summary>
    /// 测试Repository<T> Insert的功能
    /// </summary>
    [Description("测试Repository<T> Insert的功能")]
    [TestMethod]
    public void Table()
    {

      var table = repository.Table;


      Assert.AreEqual(table.Count() > 100 ? true : false, true);
    }


    #region 数据库初始化


    ///// <summary>
    ///// 数据库初始化
    ///// </summary>
    //public static void DatabaseInitialize()
    //{
    //    Database database = new  Database();

    //    string createTable_Poco = string.Empty;
    //    string dBType = ConfigurationManager.ConnectionStrings.Count == 1 ? ConfigurationManager.ConnectionStrings[0].Name : "SqlServer";

    //    switch (dBType)
    //    {
    //        case "SqlServer":
    //            createTable_Poco = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[test_SampleEntities]') AND type in (N'U'))
    //                BEGIN
    //                CREATE TABLE [dbo].[test_SampleEntities](
    //                 [Id] [bigint] IDENTITY(1,1) NOT NULL,
    //                    [UserId] [bigint] NOT NULL,
    //                 [SettingsKey] [uniqueidentifier] NOT NULL,
    //                 [Body] [nvarchar](max) NOT NULL,
    //                 [IsApproved] [bit] NOT NULL,
    //                 [Price] [decimal](18, 4) NOT NULL,
    //                 [AuditStatus] [tinyint] NOT NULL,
    //                 [DateCreated] [datetime] NOT NULL,
    //                 CONSTRAINT [PK_test_SampleEntities] PRIMARY KEY CLUSTERED (	[Id] ASC )
    //                )
    //                END
    //            ";
    //            break;
    //        case "MySql":
    //            createTable_Poco = @"
    //                CREATE TABLE IF NOT EXISTS `test_sampleentities` (
    //                  `Id` bigint(11) NOT NULL AUTO_INCREMENT,
    //                  `UserId` bigint(20) NOT NULL,
    //                  `SettingsKey` varchar(50) NOT NULL,
    //                  `Body` longtext NOT NULL,
    //                  `IsApproved` bit(1) NOT NULL,
    //                  `Price` decimal(10,3) NOT NULL,
    //                  `AuditStatus` tinyint(4) NOT NULL,
    //                  `DateCreated` datetime NOT NULL,
    //                  PRIMARY KEY (`Id`)
    //                ) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 ;";
    //            break;
    //    }
    //    database.Execute(createTable_Poco, null);
    //}
    //



    #endregion


  }
}
