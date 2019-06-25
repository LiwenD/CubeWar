using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.CubeWar
{
    public class Consts
    {
        #region Table表字段的key
        public const string TableKeyOccupation_Name = "";//Occupation表的name
        public const string CharacterModeInfoTable_Name = ""; //CharacterModeInfoTable 表的name

        public const string MapInfoTable_Length = "Length";
        public const string MapInfoTable_Width = "Width";
        //public const string MapInfoTable_StartingPoint = "";
        //public const string MapInfoTable_Destination = "";
        //public const string MapInfoTable_BattlePoint = "";
        //public const string MapInfoTable_PricePoint = "";
        //public const string MapInfoTable_BossPoint = "";
        public static readonly string[] MapInfoTableFieldName = new string[] { "StartingPoint", "Destination", "BattlePoint", "PricePoint", "BossPoint", };
        public const int MapDistance= 5;
        public const string MapParentName = "MapParent";
        /// <summary>
        /// 地图Prefab路径
        /// </summary>
        public static readonly Dictionary<GridType, string> MapPrefabPath = new Dictionary<GridType, string>()
        {
            [GridType.StartingPoint]= "Map/StartingPoint_",
            [GridType.Destination] = "Map/Destination_",
            [GridType.BattlePoint] = "Map/BattlePoint_",
            [GridType.PricePoint] = "Map/PricePoint_",
            [GridType.BossPoint] = "Map/BossPoint_",
        };
        //public static readonly Vector3 OriginPoint = Vector3.zero;
            #endregion
    }
}
