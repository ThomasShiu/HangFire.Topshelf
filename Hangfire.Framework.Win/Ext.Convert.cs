/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangfire.Topshelf.Jobs
{
  public static partial class Ext
  {
    #region 數值轉換
    /// <summary>
    /// 轉換為整型
    /// </summary>
    /// <param name="data">數據</param>
    public static int ToInt(this object data)
    {
      if (data == null)
        return 0;
      int result;
      var success = int.TryParse(data.ToString(), out result);
      if (success)
        return result;
      try
      {
        return Convert.ToInt32(ToDouble(data, 0));
      }
      catch (Exception)
      {
        return 0;
      }
    }

    /// <summary>
    /// 轉換為可空整型
    /// </summary>
    /// <param name="data">數據</param>
    public static int? ToIntOrNull(this object data)
    {
      if (data == null)
        return null;
      int result;
      bool isValid = int.TryParse(data.ToString(), out result);
      if (isValid)
        return result;
      return null;
    }

    /// <summary>
    /// 轉換為雙精度浮點數
    /// </summary>
    /// <param name="data">數據</param>
    public static double ToDouble(this object data)
    {
      if (data == null)
        return 0;
      double result;
      return double.TryParse(data.ToString(), out result) ? result : 0;
    }

    /// <summary>
    /// 轉換為雙精度浮點數,並按指定的小數位4舍5入
    /// </summary>
    /// <param name="data">數據</param>
    /// <param name="digits">小數位數</param>
    public static double ToDouble(this object data, int digits)
    {
      return Math.Round(ToDouble(data), digits);
    }

    /// <summary>
    /// 轉換為可空雙精度浮點數
    /// </summary>
    /// <param name="data">數據</param>
    public static double? ToDoubleOrNull(this object data)
    {
      if (data == null)
        return null;
      double result;
      bool isValid = double.TryParse(data.ToString(), out result);
      if (isValid)
        return result;
      return null;
    }

    /// <summary>
    /// 轉換為高精度浮點數
    /// </summary>
    /// <param name="data">數據</param>
    public static decimal ToDecimal(this object data)
    {
      if (data == null)
        return 0;
      decimal result;
      return decimal.TryParse(data.ToString(), out result) ? result : 0;
    }

    /// <summary>
    /// 轉換為高精度浮點數,並按指定的小數位4舍5入
    /// </summary>
    /// <param name="data">數據</param>
    /// <param name="digits">小數位數</param>
    public static decimal ToDecimal(this object data, int digits)
    {
      return Math.Round(ToDecimal(data), digits);
    }

    /// <summary>
    /// 轉換為可空高精度浮點數
    /// </summary>
    /// <param name="data">數據</param>
    public static decimal? ToDecimalOrNull(this object data)
    {
      if (data == null)
        return null;
      decimal result;
      bool isValid = decimal.TryParse(data.ToString(), out result);
      if (isValid)
        return result;
      return null;
    }

    /// <summary>
    /// 轉換為可空高精度浮點數,並按指定的小數位4舍5入
    /// </summary>
    /// <param name="data">數據</param>
    /// <param name="digits">小數位數</param>
    public static decimal? ToDecimalOrNull(this object data, int digits)
    {
      var result = ToDecimalOrNull(data);
      if (result == null)
        return null;
      return Math.Round(result.Value, digits);
    }

    #endregion

    #region 日期轉換
    /// <summary>
    /// 轉換為日期
    /// </summary>
    /// <param name="data">數據</param>
    public static DateTime ToDate(this object data)
    {
      if (data == null)
        return DateTime.MinValue;
      DateTime result;
      return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
    }

    /// <summary>
    /// 轉換為可空日期
    /// </summary>
    /// <param name="data">數據</param>
    public static DateTime? ToDateOrNull(this object data)
    {
      if (data == null)
        return null;
      DateTime result;
      bool isValid = DateTime.TryParse(data.ToString(), out result);
      if (isValid)
        return result;
      return null;
    }

    #endregion

    #region 布林轉換
    /// <summary>
    /// 轉換為布林值
    /// </summary>
    /// <param name="data">數據</param>
    public static bool ToBool(this object data)
    {
      if (data == null)
        return false;
      bool? value = GetBool(data);
      if (value != null)
        return value.Value;
      bool result;
      return bool.TryParse(data.ToString(), out result) && result;
    }

    /// <summary>
    /// 獲取布林值
    /// </summary>
    private static bool? GetBool(this object data)
    {
      switch (data.ToString().Trim().ToLower())
      {
        case "0":
          return false;
        case "1":
          return true;
        case "是":
          return true;
        case "否":
          return false;
        case "yes":
          return true;
        case "no":
          return false;
        default:
          return null;
      }
    }

    /// <summary>
    /// 轉換為可空布林值
    /// </summary>
    /// <param name="data">數據</param>
    public static bool? ToBoolOrNull(this object data)
    {
      if (data == null)
        return null;
      bool? value = GetBool(data);
      if (value != null)
        return value.Value;
      bool result;
      bool isValid = bool.TryParse(data.ToString(), out result);
      if (isValid)
        return result;
      return null;
    }

    #endregion

    #region 字串轉換
    /// <summary>
    /// 轉換為字串
    /// </summary>
    /// <param name="data">數據</param>
    public static string ToString(this object data)
    {
      return data == null ? string.Empty : data.ToString().Trim();
    }
    #endregion

    /// <summary>
    /// 安全返回值
    /// </summary>
    /// <param name="value">可空值</param>
    public static T SafeValue<T>(this T? value) where T : struct
    {
      return value ?? default(T);
    }
    /// <summary>
    /// 是否為空
    /// </summary>
    /// <param name="value">值</param>
    public static bool IsEmpty(this string value)
    {
      return string.IsNullOrWhiteSpace(value);
    }
    /// <summary>
    /// 是否為空
    /// </summary>
    /// <param name="value">值</param>
    public static bool IsEmpty(this Guid? value)
    {
      if (value == null)
        return true;
      return IsEmpty(value.Value);
    }
    /// <summary>
    /// 是否為空
    /// </summary>
    /// <param name="value">值</param>
    public static bool IsEmpty(this Guid value)
    {
      if (value == Guid.Empty)
        return true;
      return false;
    }
    /// <summary>
    /// 是否為空
    /// </summary>
    /// <param name="value">值</param>
    public static bool IsEmpty(this object value)
    {
      if (value != null && !string.IsNullOrEmpty(value.ToString()))
      {
        return false;
      }
      else
      {
        return true;
      }
    }
  }
}

