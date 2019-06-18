using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.CubeWar
{
    /// <summary>
    /// 配置表类型，用来表示需要取得哪个配置表
    /// </summary>
    public enum TableType
    {
        BulletTable = 0,
        CharacterModeInfoTable,
        OccupationTable,
        Weapontable,
        Max,
    }

    /// <summary>
    /// 职业
    /// </summary>
    public enum OccupationType
    {
        Soldier = 1,
        Boomerman,
        Max,
    }
}
