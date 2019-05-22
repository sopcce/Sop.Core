

namespace Sop.Common.Helper.Extensions
{
    /// <summary>
    /// 提供用于设置转换选项的枚举值。
    /// </summary>
    /// <value>
    /// <code>FirstLetterOnly</code>
    /// 只转换拼音首字母，默认转换全部
    /// </value>
    /// <value>
    /// <code>TranslateUnknowWordToInterrogation</code>
    /// 转换未知汉字为问号，默认不转换
    /// </value>
    /// <value>
    /// <code>EnableUnicodeLetter</code>
    /// 保留非字母、非数字字符，默认不保留
    /// </value>
    [System.FlagsAttribute]
    public enum ChinesePinYinOptions
    {
        /// <summary>
        /// 只转换拼音首字母，默认转换全部
        /// </summary>
        FirstLetterOnly = 1,
        /// <summary>
        /// 转换未知汉字为问号，默认不转换
        /// </summary>
        TranslateUnknowWordToInterrogation = 1 << 1,
        /// <summary>
        /// 保留非字母、非数字字符，默认不保留
        /// </summary>
        EnableUnicodeLetter = 1 << 2,							 
    }

}
