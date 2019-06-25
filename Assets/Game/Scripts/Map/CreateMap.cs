using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using YummyGame.Framework;

using Random = UnityEngine.Random;

namespace YummyGame.CubeWar
{
    public struct MapInfo
    {
        public int Length;
        public int Width;
        public int[] PointsCount;
        //public int StartingPoint;
        //public int Destination;
        //public int BattlePoint;
        //public int PricePoint;
        //public int BossPoint;
    }
    public class CreateMap
    {
        CubeWarManager cubeWarManager;
        TableManager tableManager;

        DataTable mapTable;
        MapInfo mapInfo;
        MapGrid[,] mapGrids;
        List<IndexInfo> mapInfoCache = new List<IndexInfo>();
        Dictionary<GridType, int> gridTypeCount = new Dictionary<GridType, int>();//记录各种grid还有多少点没有生成
        Func<GameObject, GameObject> instantiationMapFunc; //实例化地图的委托
        Func<GridType, string> randomMapNmae;              //随机取得GridType这个类型地图的name
        Transform mapParent;

        public void Init(CubeWarManager cbm, Func<GameObject, GameObject> funcinstantiationMapFunc, Func<GridType, string> funcrandomMapNmae)
        {
            mapInfoCache.Clear();     //清理之前留下的缓存
            gridTypeCount.Clear();

            cubeWarManager = cbm;
            instantiationMapFunc = funcinstantiationMapFunc;
            randomMapNmae = funcrandomMapNmae;

            tableManager = cubeWarManager.MTableManager;
            mapTable = tableManager.GetTable(TableType.MapTable);

            mapParent = new GameObject(Consts.MapParentName).transform;
            mapParent.position = Vector3.zero;

            SetInfo();
            mapGrids = new MapGrid[mapInfo.Length, mapInfo.Width];
            CreateInfo();
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

        public IEnumerator InstantiationMap()
        {
            AssetLoader assetLoader = new AssetLoader();
            int pause = 0;

            foreach (var item in mapInfoCache)//生成地图物体
            {
                string prefabName = randomMapNmae?.Invoke(mapGrids[item.x, item.y].gridType);
                GameObject mapPrefab = assetLoader.LoadAsset<GameObject>(prefabName);
                GameObject mapGo = instantiationMapFunc?.Invoke(mapPrefab);
                mapGo.transform.SetParent(mapParent);
                mapGo.transform.localPosition = new Vector3(Consts.MapDistance * item.x, 0, Consts.MapDistance * item.y);
                mapGrids[item.x, item.y].mapGo = mapGo;
                mapGrids[item.x, item.y].IsInited = true;

                pause++;
                if (pause >= 5)
                {
                    pause = 0;
                    yield return null;
                }
            }

            foreach (var item in mapInfoCache)//生成地图走廊
            {
                Map map = mapGrids[item.x, item.y].mapGo.AddComponent<Map>();
                map.Spawncorridor(mapGrids[item.x, item.y]);

                pause++;
                if (pause >= 5)
                {
                    pause = 0;
                    yield return null;
                }
            }
        }

        #region 私有
        /// <summary>
        /// 设置地图信息
        /// </summary>
        void SetInfo()
        {
            mapInfo = new MapInfo() { PointsCount = new int[Consts.MapInfoTableFieldName.Length] };

            int tableRow = ((StaticData.CurLevel - 1) * 5) + StaticData.CurLevelChild;   //*5是因为每个大关卡的子关卡是5关

            mapInfo.Length = mapTable.Get<int>(tableRow, Consts.MapInfoTable_Length);
            mapInfo.Width = mapTable.Get<int>(tableRow, Consts.MapInfoTable_Width);
            #region 
            //mapInfo.StartingPoint = mapTable.Get<int>(tableRow, Consts.MapInfoTable_StartingPoint);
            //mapInfo.Destination = mapTable.Get<int>(tableRow, Consts.MapInfoTable_Destination);
            //mapInfo.BattlePoint = mapTable.Get<int>(tableRow, Consts.MapInfoTable_BattlePoint);
            //mapInfo.PricePoint = mapTable.Get<int>(tableRow, Consts.MapInfoTable_PricePoint);
            //mapInfo.BossPoint = mapTable.Get<int>(tableRow, Consts.MapInfoTable_BossPoint);
            //StartingPoint,
            //Destination,
            //BattlePoint,
            //PricePoint,
            //BossPoint,
            #endregion
            for (int i = 0; i < Consts.MapInfoTableFieldName.Length; i++)
            {
                mapInfo.PointsCount[i] = mapTable.Get<int>(tableRow, Consts.MapInfoTableFieldName[i]);
                gridTypeCount.Add((GridType)i, mapInfo.PointsCount[i]);
            }
        }

        bool lastGridIsPrice = false;  //表示上一次生成的GridType是不是PricePoint
        void CreateInfo()
        {
            RandomStartingPoint();

            while (true)
            {
                int lessen = 1;
                while (GetGrideAroundNullCount(mapInfoCache[mapInfoCache.Count - lessen]) > 0)//判断这个点周围是否还能生成点
                {
                    lessen++;
                    if (mapInfoCache.Count - lessen < 0)
                    {
                        Debug.LogException(new System.Exception("创建地图信息出错！"));
                        return;
                    }
                }
                IndexInfo temp = RandomPointDir(mapInfoCache[mapInfoCache.Count - lessen]);
                mapInfoCache.Add(temp);
                mapGrids[temp.x, temp.y] = new MapGrid();
                mapGrids[temp.x, temp.y].indexInfo = temp;
                GridType tempGridType = RandomGridType();
                while (lastGridIsPrice)
                {
                    tempGridType = RandomGridType();
                    if (tempGridType != GridType.PricePoint)
                    {
                        break;
                    }
                }
                lastGridIsPrice = tempGridType == GridType.PricePoint ? true : false;
                mapGrids[temp.x, temp.y].gridType = tempGridType;
                gridTypeCount[mapGrids[temp.x, temp.y].gridType]--;
                if (IsCreated())//grid信息生成完毕
                {
                    break;
                }
            }

            //设置所有Grid的上下左右点的信息
            SetGridInfo();

            //实例化地图PrefabTODO
        }

        /// <summary>
        /// 随机取得一个起点
        /// </summary>
        void RandomStartingPoint()
        {
            int col = Random.Range(0, mapInfo.Length);
            int row = Random.Range(0, 2) == 0 ? 0 : mapInfo.Width - 1;
            mapGrids[col, row] = new MapGrid();
            mapGrids[col, row].gridType = GridType.StartingPoint;
            mapGrids[col, row].indexInfo.x = col;
            mapGrids[col, row].indexInfo.y = row;
            mapInfoCache.Add(mapGrids[col, row].indexInfo);
            gridTypeCount[GridType.StartingPoint]--;
        }

        IndexInfo RandomPointDir(IndexInfo indexInfo)
        {
            int randomDir = -1;
            IndexInfo temp;
            while (true)
            {
                temp = new IndexInfo() { x = indexInfo.x, y = indexInfo.y };
                randomDir = Random.Range(0, 4);  //0-3分别表示Up Down Left Right四个方向
                switch (randomDir)
                {
                    case 0:
                        temp.y -= 1;
                        break;
                    case 1:
                        temp.y += 1;
                        break;
                    case 2:
                        temp.x -= 1;
                        break;
                    case 3:
                        temp.x += 1;
                        break;
                    default:
                        break;
                }
                if (!CheckIsOutRange(temp) && mapGrids[temp.x, temp.y] != null)
                {
                    return temp;
                }
            }
        }

        /// <summary>
        /// 检测下标是否超过mapGrids的边界
        /// </summary>
        /// <param name="indexInfo"></param>
        /// <returns>True - 越界，False-未越界 </returns>
        bool CheckIsOutRange(IndexInfo indexInfo)
        {
            return CheckIsOutRange(indexInfo.x, indexInfo.y);
        }

        /// <summary>
        /// 检测下标是否超过mapGrids的边界
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True - 越界，False-未越界</returns>
        bool CheckIsOutRange(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return true;
            }
            else if (x >= mapInfo.Length || y >= mapInfo.Width)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 随机Grid类型
        /// </summary>
        /// <returns></returns>
        GridType RandomGridType()
        {
            while (true)
            {
                int temp = Random.Range((int)GridType.Destination, (int)GridType.Max);
                if (gridTypeCount[(GridType)temp] > 0)
                {
                    return (GridType)temp;
                }
            }
        }

        /// <summary>
        /// 地图信息是否创建完成
        /// </summary>
        /// <returns></returns>
        bool IsCreated()
        {
            foreach (var item in gridTypeCount)
            {
                if (item.Value > 0) return false;
            }
            return true;
        }

        /// <summary>
        /// 得到这个格子空方向数量
        /// </summary>
        /// <param name="indexInfo"></param>
        /// <returns></returns>
        int GetGrideAroundNullCount(IndexInfo indexInfo)
        {
            int count = 0;
            IndexInfo temp;
            for (int i = 0; i < 4; i++)
            {
                temp = new IndexInfo() { x = indexInfo.x, y = indexInfo.y };
                switch (i)
                {
                    case 0://Up
                        temp.y -= 1;
                        break;
                    case 1://Down
                        temp.y += 1;
                        break;
                    case 2://Left
                        temp.x -= 1;
                        break;
                    case 3://Right
                        temp.x += 1;
                        break;
                    default:
                        break;
                }
                if (CheckIsOutRange(temp) && mapGrids[temp.x, temp.y] == null)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 设置格子的Up Down Left Right
        /// </summary>
        void SetGridInfo()
        {
            foreach (var item in mapInfoCache)
            {
                if (!CheckIsOutRange(item.x, item.y - 1) && mapGrids[item.x, item.y - 1] != null)//Up
                {
                    mapGrids[item.x, item.y].Up = mapGrids[item.x, item.y - 1];
                }
                else
                {
                    mapGrids[item.x, item.y].Up = null;
                }

                if (!CheckIsOutRange(item.x, item.y + 1) && mapGrids[item.x, item.y + 1] != null)//Down
                {
                    mapGrids[item.x, item.y].Down = mapGrids[item.x, item.y + 1];
                }
                else
                {
                    mapGrids[item.x, item.y].Down = null;
                }

                if (!CheckIsOutRange(item.x - 1, item.y) && mapGrids[item.x - 1, item.y] != null)//Left
                {
                    mapGrids[item.x, item.y].Left = mapGrids[item.x - 1, item.y];
                }
                else
                {
                    mapGrids[item.x, item.y].Left = null;
                }

                if (!CheckIsOutRange(item.x + 1, item.y) && mapGrids[item.x + 1, item.y] != null)//Right
                {
                    mapGrids[item.x, item.y].Right = mapGrids[item.x + 1, item.y];
                }
                else
                {
                    mapGrids[item.x, item.y].Right = null;
                }
            }
        }

        #endregion
    }
}