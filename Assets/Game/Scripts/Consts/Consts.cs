using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.CubeWar
{
    public class Consts
    {
        #region Table表字段的key
        #endregion
        public const string TableKeyOccupation_Name = "";//Occupation表的name
        public const string CharacterModeInfoTable_Name = ""; //CharacterModeInfoTable 表的name

        #region 地图生成
        /// <summary>
        /// 允许生成终点的剩余长度
        /// </summary>
        public const int AllowSpawnDestination = 3;
        public const string MapInfoTable_Length = "Length";
        public const string MapInfoTable_Width = "Width";
        //public const string MapInfoTable_StartingPoint = "";
        //public const string MapInfoTable_Destination = "";
        //public const string MapInfoTable_BattlePoint = "";
        //public const string MapInfoTable_PricePoint = "";
        //public const string MapInfoTable_BossPoint = "";
        public static readonly string[] MapInfoTableFieldName = new string[] { "StartingPoint", "Destination", "BattlePoint", "PricePoint", "BossPoint", };
        public const int MapDistance= 40;
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
        public const string CorridorPath = "Map/CorridorPrefab";//地图的走廊路径
        public const string CorridorGroundPath = "Map/TestGround";            //走廊地板元素prefab
        public const string CorridorWallPath = "Map/TestWall";              //走廊墙元素路径
        #endregion
    }
}
