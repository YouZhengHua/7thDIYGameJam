using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 計算工具
/// 輔助計算各種數值使用
/// </summary>
public static class CalTool
{
    /// <summary>
    /// 取得計算防禦減傷後的傷害值
    /// </summary>
    /// <param name="originalDamage">原始傷害值</param>
    /// <param name="def">防禦力</param>
    /// <param name="minDamage">最小傷害值，預設為 1</param>
    /// <returns></returns>
    public static float CalDamage(float originalDamage, float def, float minDamage = 1f)
    {
        float calDamage = originalDamage - def;
        if (calDamage < minDamage)
            return minDamage;
        return CalTool.Round(calDamage, 1);
    }

    /// <summary>
    /// 四捨五入至指定位數
    /// </summary>
    /// <param name="f"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static float Round(float f, int d = 0)
    {
        d = d < 0 ? 0 : d;
        float pow = Mathf.Pow(10, d);
        return Mathf.Round(f * pow) / pow;
    }
}
