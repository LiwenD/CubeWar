using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class V3ExpandFunc
{
    /// <summary>
    /// 求两个点的中间点
    /// </summary>
    /// <param name="v1">第一点</param>
    /// <param name="v2">第二点</param>
    /// <returns></returns>
    public static Vector3 GetMidPoint(this Vector3 v1, Vector3 v2)
    {
        Vector3 nor = (v2 - v1).normalized;
        float dis = Vector3.Distance(v1, v2);

        return nor * (dis / 2) + v1;
    }
}
