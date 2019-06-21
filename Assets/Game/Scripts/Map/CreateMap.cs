using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public struct MapInfo
    {
        public int Length;
        public int Width;
        public int StartingPoint;
        public int Destination;
        public int BattlePoint;
        public int PricePoint;
        public int BossPoint;
    }
    public class CreateMap
    {
        CubeWarManager cubeWarManager;
        TableManager tableManager;

        DataTable mapTable;
        MapInfo mapInfo;
        MapGrid[,] mapGrids;
        List<IndexInfo> mapInfoCache = new List<IndexInfo>();

        public void Init(CubeWarManager cbm, int level)
        {
            mapInfoCache.Clear();     //清理之前留下的缓存
            cubeWarManager = cbm;
            tableManager = cubeWarManager.MTableManager;
            mapTable = tableManager.GetTable(TableType.MapTable);
            SetInfo();
            mapGrids = new MapGrid[mapInfo.Length, mapInfo.Width];

            #region 
            /*读取配置表，取得这一关的关卡信息，然后开始随机生成地图
            表的信息有：矩形数组的长宽，奖励点数量，怪物点数量，终点，起点*/

            /*
             * 生成逻辑：
             * 从矩形数组里面随机取一个下标（取的条件必须是矩形数组的边界）当作出生点，开始生成，用Random.Range返回0-3范围的数字（0上 1下 2左 3右）确定下一个Grid，取到随机的方向后，判断这个下标是否超过矩形数组的边界，如果超过，则重新生成一次。
             * 生成Grid之后，则从配置表中取得当前表信息的各个点数量，随机取一个点出来用来确定这个点是什么类型的点（怪物，奖励等！注意：除怪物之外的点不能连续生成两次，需要隔一次）
             * 
             * 遍历矩形数组的所有节点，然后判断这个节点的上下左右是否有节点，如果有则记录，并且把没有节点的方向的开口封闭
             * */
            #endregion
        }

        /// <summary>
        /// 设置地图信息
        /// </summary>
        void SetInfo()
        {
            mapInfo = new MapInfo();

            int tableRow = ((StaticData.CurLevel - 1) * 5) + StaticData.CurLevelChild;   //*5是因为每个大关卡的子关卡是5关

            mapInfo.Length = mapTable.Get<int>(tableRow, Consts.MapInfoTable_Length);
            mapInfo.Width = mapTable.Get<int>(tableRow, Consts.MapInfoTable_Width);
            mapInfo.StartingPoint= mapTable.Get<int>(tableRow, Consts.MapInfoTable_StartingPoint);
            mapInfo.Destination = mapTable.Get<int>(tableRow, Consts.MapInfoTable_Destination);
            mapInfo.BattlePoint = mapTable.Get<int>(tableRow, Consts.MapInfoTable_BattlePoint);
            mapInfo.PricePoint = mapTable.Get<int>(tableRow, Consts.MapInfoTable_PricePoint);
            mapInfo.BossPoint = mapTable.Get<int>(tableRow, Consts.MapInfoTable_BossPoint);
        }

        void CreateInfo()
        {
            RandomStartingPoint();
        }

        void RandomStartingPoint()
        {
            int col = Random.Range(0, mapInfo.Length);
            int row = Random.Range(0, 2) == 0 ? 0 : mapInfo.Width - 1;
            mapGrids[col, row] = new MapGrid();
            mapGrids[col, row].gridType = GridType.StartingPoint;
            mapGrids[col, row].indexInfo.col = col;
            mapGrids[col, row].indexInfo.row = row;
            mapInfoCache.Add(mapGrids[col, row].indexInfo);
        }
    }
}