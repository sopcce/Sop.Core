using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Framework.Test.Models
{
  /// <summary>
  /// Sop_Test测试数据实体类
  /// </summary>
  [SopTableName("Item_TestInfo")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]  
  [Serializable]
  public class TestInfo
  {
    public TestInfo()
    {
      Type = TestInfoType.Success;
      DateCreated = DateTime.Now;
    }
    public int Id { get; set; }
    /// <summary>
    /// Type
    /// </summary>		
    public TestInfoType Type { get; set; }

    /// <summary>
    /// IsDel
    /// </summary>		
    public bool IsDel { get; set; }

    /// <summary>
    /// Status
    /// </summary>		
    public bool Status { get; set; }

    /// <summary>
    /// LongValue
    /// </summary>		
    public long LongValue { get; set; }

    /// <summary>
    /// FloatValue
    /// </summary>		
    public float FloatValue { get; set; }

    /// <summary>
    /// DecimalValue
    /// </summary>		
    public decimal DecimalValue { get; set; }

    /// <summary>
    /// Body
    /// </summary>		
    public string Body { get; set; }

    /// <summary>
    /// DateCreated
    /// </summary>		
    public DateTime DateCreated { get; set; }


  }


  /// <summary>
  /// 审核状态
  /// </summary>
  public enum TestInfoType
  {
    /// <summary>
    /// 未通过
    /// </summary>
    Fail = 10,

    /// <summary>
    /// 待审核
    /// </summary>
    Pending = 20,

    /// <summary>
    /// 需再次审核
    /// </summary>
    Again = 30,

    /// <summary>
    /// 通过验证
    /// </summary>
    Success = 40
  }

}