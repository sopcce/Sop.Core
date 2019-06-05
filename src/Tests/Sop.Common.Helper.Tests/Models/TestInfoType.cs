namespace Sop.Common.Helper.Tests.Models
{
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