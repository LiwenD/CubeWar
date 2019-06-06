using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{

    /// <summary>
    /// 此等级是花费游戏币所升级，游戏内是不升级的！
    /// </summary>
    public int LV { get; set; }

    public int HP { get; set; }

    public int MP { get; set; }

    public int Armor { get; set; }

    /// <summary>
    /// 表示是否能恢复Armor
    /// </summary>
    public bool IsCanRecoverArmor { get; set; }

    /// <summary>
    /// 每秒护甲恢复速度
    /// </summary>
    public const int RoceverArmorSpeed = 1;

    public void AddHP(int val)
    {
        HP += val;
    }

    /// <summary>
    /// 返回False表示HP<=0
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public bool LessHP(int val)
    {
        IsCanRecoverArmor = false;
        HP -= val;
        return HP > 0;
    }

    public void AddMP(int val)
    {
        MP += val;
    }

    /// <summary>
    /// 返回False表示MP不足
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public bool LessMP(int val)
    {
        if (MP - val > 0)
        {
            MP -= val;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddArmor(int val)
    {
        Armor += val;
    }

    /// <summary>
    /// 返回False表示HP<=0
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public bool LessArmor(int val)
    {
        IsCanRecoverArmor = false;
        if (Armor - val > 0)
        {
            Armor -= val;
            return true;
        }
        else
        {
            Armor = 0;
            int tempI = Armor - val;
            return LessHP(Mathf.Abs(tempI));
        }
    }
}
