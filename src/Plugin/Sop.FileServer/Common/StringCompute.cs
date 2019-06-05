using System;

namespace URun.WXBSnap.Logic
{
  public class StringCompute
  {
    /// <summary>
    /// 比较二个字符串
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="isCheck">是否按照相似对判断，差异字符小于20且相似度大于0.3</param>
    /// <returns></returns>
    public static bool StrContains(string str1, string str2, bool isCheck = true)
    {
      if (string.IsNullOrWhiteSpace(str1) || string.IsNullOrWhiteSpace(str1))
        return false;
      if (str1.Contains(str2))
        return true;
      else if (str2.Contains(str1))
        return true;
      int shortValue = str1.Length > str2.Length ? str2.Length : str1.Length;
      string str1Temp = str1, str2Temp = str2;

      //在判断
      int str2StartIndex = str2Temp.IndexOf(str1.Substring(0, 1), StringComparison.OrdinalIgnoreCase);

      int length1 = str2Temp.Length - (str2StartIndex > 0 ? str2StartIndex : 0);
      if (str2StartIndex > 0)
      {
        str2Temp = str2Temp.Substring(str2StartIndex, length1);
        if (str1.Contains(str2Temp))
          return true;
      }
      //再一次判断
      int str1StartIndex = str1Temp.IndexOf(str2.Substring(0, 1), StringComparison.OrdinalIgnoreCase);
      int length2 = str1Temp.Length - (str1StartIndex > 0 ? str1StartIndex : 0);
      if (str1StartIndex > 0)
      {
        str1Temp = str1Temp.Substring(str1StartIndex, length2);
        if (str2.Contains(str1Temp))
          return true;
      }
      if (isCheck)
      {
        StringCompute stringCompute = new StringCompute();
        stringCompute.SpeedyCompute(str1, str2);
        decimal de = stringCompute.ComputeResult.Rate;
        if (stringCompute.ComputeResult.Difference < 50 && de.CompareTo(0.3M) > 0)
        {//stringCompute.ComputeResult.Difference < 20
          return true;
        }
      }
      return false;

    }

    #region 私有变量
    /// <summary>
    /// 字符串1
    /// </summary>
    private char[] _ArrChar1;
    /// <summary>
    /// 字符串2
    /// </summary>
    private char[] _ArrChar2;
    /// <summary>
    /// 统计结果
    /// </summary>
    private Result _Result;
    /// <summary>
    /// 开始时间
    /// </summary>
    private DateTime _BeginTime;
    /// <summary>
    /// 结束时间
    /// </summary>
    private DateTime _EndTime;
    /// <summary>
    /// 计算次数
    /// </summary>
    private int _ComputeTimes;
    /// <summary>
    /// 算法矩阵
    /// </summary>
    private int[,] _Matrix;
    /// <summary>
    /// 矩阵列数
    /// </summary>
    private int _Column;
    /// <summary>
    /// 矩阵行数
    /// </summary>
    private int _Row;
    #endregion
    #region 属性
    public Result ComputeResult
    {
      get { return _Result; }
    }
    #endregion
    #region 构造函数
    public StringCompute(string str1, string str2)
    {
      this.StringComputeInit(str1, str2);
    }
    public StringCompute()
    {
    }
    #endregion
    #region 算法实现
    /// <summary>
    /// 初始化算法基本信息
    /// </summary>
    /// <param name="str1">字符串1</param>
    /// <param name="str2">字符串2</param>
    private void StringComputeInit(string str1, string str2)
    {
      _ArrChar1 = str1.ToCharArray();
      _ArrChar2 = str2.ToCharArray();
      _Result = new Result();
      _ComputeTimes = 0;
      _Row = _ArrChar1.Length + 1;
      _Column = _ArrChar2.Length + 1;
      _Matrix = new int[_Row, _Column];
    }
    /// <summary>
    /// 计算相似度
    /// </summary>
    public void Compute()
    {
      //开始时间
      _BeginTime = DateTime.Now;
      //初始化矩阵的第一行和第一列
      this.InitMatrix();
      int intCost = 0;
      for (int i = 1; i < _Row; i++)
      {
        for (int j = 1; j < _Column; j++)
        {
          if (_ArrChar1[i - 1] == _ArrChar2[j - 1])
          {
            intCost = 0;
          }
          else
          {
            intCost = 1;
          }
          //关键步骤，计算当前位置值为左边+1、上面+1、左上角+intCost中的最小值 
          //循环遍历到最后_Matrix[_Row - 1, _Column - 1]即为两个字符串的距离
          _Matrix[i, j] = this.Minimum(_Matrix[i - 1, j] + 1, _Matrix[i, j - 1] + 1, _Matrix[i - 1, j - 1] + intCost);
          _ComputeTimes++;
        }
      }
      //结束时间
      _EndTime = DateTime.Now;
      //相似率 移动次数小于最长的字符串长度的20%算同一题
      int intLength = _Row > _Column ? _Row : _Column;

      _Result.Rate = (1 - (decimal)_Matrix[_Row - 1, _Column - 1] / intLength);
      _Result.UseTime = (_EndTime - _BeginTime).ToString();
      _Result.ComputeTimes = _ComputeTimes.ToString();
      _Result.Difference = _Matrix[_Row - 1, _Column - 1];
    }


    /// <summary>
    /// 计算相似度（不记录比较时间）
    /// </summary>
    public void SpeedyCompute()
    {
      //开始时间
      //_BeginTime = DateTime.Now;
      //初始化矩阵的第一行和第一列
      this.InitMatrix();
      int intCost = 0;
      for (int i = 1; i < _Row; i++)
      {
        for (int j = 1; j < _Column; j++)
        {
          if (_ArrChar1[i - 1] == _ArrChar2[j - 1])
          {
            intCost = 0;
          }
          else
          {
            intCost = 1;
          }
          //关键步骤，计算当前位置值为左边+1、上面+1、左上角+intCost中的最小值 
          //循环遍历到最后_Matrix[_Row - 1, _Column - 1]即为两个字符串的距离
          _Matrix[i, j] = this.Minimum(_Matrix[i - 1, j] + 1, _Matrix[i, j - 1] + 1, _Matrix[i - 1, j - 1] + intCost);
          _ComputeTimes++;
        }
      }
      //结束时间
      //_EndTime = DateTime.Now;
      //相似率 移动次数小于最长的字符串长度的20%算同一题
      int intLength = _Row > _Column ? _Row : _Column;

      _Result.Rate = (1 - (decimal)_Matrix[_Row - 1, _Column - 1] / intLength);
      // _Result.UseTime = (_EndTime - _BeginTime).ToString();
      _Result.ComputeTimes = _ComputeTimes.ToString();
      _Result.Difference = _Matrix[_Row - 1, _Column - 1];
    }
    /// <summary>
    /// 计算相似度
    /// </summary>
    /// <param name="str1">字符串1</param>
    /// <param name="str2">字符串2</param>
    public void Compute(string str1, string str2)
    {
      this.StringComputeInit(str1, str2);
      this.Compute();
    }

    /// <summary>
    /// 计算相似度
    /// </summary>
    /// <param name="str1">字符串1</param>
    /// <param name="str2">字符串2</param>
    public void SpeedyCompute(string str1, string str2)
    {
      this.StringComputeInit(str1, str2);
      this.SpeedyCompute();
    }
    /// <summary>
    /// 初始化矩阵的第一行和第一列
    /// </summary>
    private void InitMatrix()
    {
      for (int i = 0; i < _Column; i++)
      {
        _Matrix[0, i] = i;
      }
      for (int i = 0; i < _Row; i++)
      {
        _Matrix[i, 0] = i;
      }
    }
    /// <summary>
    /// 取三个数中的最小值
    /// </summary>
    /// <param name="First"></param>
    /// <param name="Second"></param>
    /// <param name="Third"></param>
    /// <returns></returns>
    private int Minimum(int First, int Second, int Third)
    {
      int intMin = First;
      if (Second < intMin)
      {
        intMin = Second;
      }
      if (Third < intMin)
      {
        intMin = Third;
      }
      return intMin;
    }
    #endregion
  }
  /// <summary>
  /// 计算结果
  /// </summary>
  public struct Result
  {
    /// <summary>
    /// 相似度
    /// </summary>
    public decimal Rate;
    /// <summary>
    /// 对比次数
    /// </summary>
    public string ComputeTimes;
    /// <summary>
    /// 使用时间
    /// </summary>
    public string UseTime;
    /// <summary>
    /// 差异
    /// </summary>
    public int Difference;
  }
}
