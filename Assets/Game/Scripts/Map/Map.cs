using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public class Map : MonoBehaviour
    {
        /// <summary>
        /// 走廊的朝向
        /// </summary>
        enum CorridorDirectionm
        {
            Up,
            Down,
            Left,
            Right,
            Max,
        }

        AssetLoader assetLoader;//= new AssetLoader();
        //MapGrid mp = new MapGrid();

        /// <summary>
        /// 生成走廊
        /// </summary>
        /// <param name="mapGrid"></param>
        /// <param name="mapParent"></param>
        public void SpawnCorridor(MapGrid mapGrid, Transform mapParent)
        {
            assetLoader = new AssetLoader();
            GameObject corridorPrefab = assetLoader.LoadAsset<GameObject>(Consts.CorridorPath);
            //Vector3 pos;

            //if (MapGrid.CheckCorridor(mapGrid.Up))
            //{
            //    pos = V3ExpandFunc.GetMidPoint(mapGrid.MapGo.transform.localPosition, mapGrid.Up.MapGo.transform.localPosition);
            //    InstantiateCorridor(corridorPrefab, mapParent, pos, mapGrid, CorridorDirectionm.Up);
            //}
            //CloseCap(corridorPrefab, mapParent, mapGrid.UpTab, mapGrid.Up, true);
            CheckIsTrueSpawn(mapGrid, mapGrid.Up, corridorPrefab, mapParent, CorridorDirectionm.Up, true);

            //if (MapGrid.CheckCorridor(mapGrid.Down))
            //{
            //    pos = V3ExpandFunc.GetMidPoint(mapGrid.MapGo.transform.localPosition, mapGrid.Down.MapGo.transform.localPosition);
            //    InstantiateCorridor(corridorPrefab, mapParent, pos, mapGrid, CorridorDirectionm.Down);
            //}
            //CloseCap(corridorPrefab, mapParent, mapGrid.DownTab, mapGrid.Down, true);
            CheckIsTrueSpawn(mapGrid, mapGrid.Down, corridorPrefab, mapParent, CorridorDirectionm.Down, true);

            //if (MapGrid.CheckCorridor(mapGrid.Left))
            //{
            //    pos = V3ExpandFunc.GetMidPoint(mapGrid.MapGo.transform.localPosition, mapGrid.Left.MapGo.transform.localPosition);
            //    InstantiateCorridor(corridorPrefab, mapParent, pos, mapGrid, CorridorDirectionm.Left);
            //}
            //CloseCap(corridorPrefab, mapParent, mapGrid.LeftTab, mapGrid.Left, false);
            CheckIsTrueSpawn(mapGrid, mapGrid.Left, corridorPrefab, mapParent, CorridorDirectionm.Left, false);

            //if (MapGrid.CheckCorridor(mapGrid.Right))
            //{
            //    pos = V3ExpandFunc.GetMidPoint(mapGrid.MapGo.transform.localPosition, mapGrid.Right.MapGo.transform.localPosition);
            //    InstantiateCorridor(corridorPrefab, mapParent, pos, mapGrid, CorridorDirectionm.Right);
            //}
            //CloseCap(corridorPrefab, mapParent, mapGrid.RightTab, mapGrid.Right, false);
            CheckIsTrueSpawn(mapGrid, mapGrid.Right, corridorPrefab, mapParent, CorridorDirectionm.Right, false);
        }

        void CheckIsTrueSpawn(MapGrid thisMG, MapGrid dirMG, GameObject corridorPrefab, Transform mapParent, CorridorDirectionm dirEnum, bool isX)
        {
            if (MapGrid.CheckCorridor(dirMG))
            {
                Vector3 pos = V3ExpandFunc.GetMidPoint(thisMG.MapGo.transform.localPosition, dirMG.MapGo.transform.localPosition);
                InstantiateCorridor(corridorPrefab, mapParent, pos, thisMG, dirEnum);
            }
            Transform corTrans = null;
            switch (dirEnum)
            {
                case CorridorDirectionm.Up:
                    corTrans = thisMG.UpTab;
                    break;
                case CorridorDirectionm.Down:
                    corTrans = thisMG.DownTab;
                    break;
                case CorridorDirectionm.Left:
                    corTrans = thisMG.LeftTab;
                    break;
                case CorridorDirectionm.Right:
                    corTrans = thisMG.RightTab;
                    break;
                default:
                    Debug.LogError(dirEnum);
                    return;
            }
            CloseCap(corridorPrefab, mapParent, corTrans, dirMG, isX);
        }

        /// <summary>
        /// 实例化走廊
        /// </summary>
        /// <param name="corridorPrefab"></param>
        /// <param name="par"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        void InstantiateCorridor(GameObject corridorPrefab, Transform par, Vector3 v3, MapGrid mapGrid, CorridorDirectionm dir)
        {
            GameObject corridor = Instantiate(corridorPrefab);
            corridor.transform.SetParent(par);
            corridor.transform.localPosition = v3;
            Transform groundP = corridor.transform.Find("Ground");
            Transform wallP = corridor.transform.Find("Wall");
            InstantiateDorectionElem(mapGrid, groundP, wallP, dir);//mapGrid.RightTab, mapGrid.Right.LeftTab
        }

        /// <summary>
        /// 根据走廊的朝向实例化走廊元素
        /// </summary>
        /// <param name="dir">朝向</param>
        void InstantiateDorectionElem(MapGrid mapGrid, Transform groundParent, Transform wallParent, CorridorDirectionm dir)
        {
            //Transform t1, Transform t2
            switch (dir)
            {
                case CorridorDirectionm.Up:
                    //Debug.Log(mapGrid.MapGo.transform.localPosition);
                    //Debug.Log(mapGrid.Up.MapGo.transform.localPosition);
                    SpawnCorriderElem(mapGrid.Up.DownTab, mapGrid.UpTab, groundParent, wallParent, false);//因为函数里面写的是计算是Z--，所以计算上边需要t2,t1
                    break;
                case CorridorDirectionm.Down:
                    //Debug.Log(mapGrid.MapGo.transform.localPosition);
                    //Debug.Log(mapGrid.Down.MapGo.transform.localPosition);
                    SpawnCorriderElem(mapGrid.DownTab, mapGrid.Down.UpTab, groundParent, wallParent, false);
                    break;
                case CorridorDirectionm.Left:
                    //Debug.Log(mapGrid.MapGo.transform.localPosition);
                    //Debug.Log(mapGrid.Left.MapGo.transform.localPosition);
                    SpawnCorriderElem(mapGrid.LeftTab, mapGrid.Left.RightTab, groundParent, wallParent, true);
                    break;
                case CorridorDirectionm.Right:
                    Debug.Log(mapGrid.Right);
                    Debug.Log(mapGrid.Right.MapGo);
                    SpawnCorriderElem(mapGrid.Right.LeftTab, mapGrid.RightTab, groundParent, wallParent, true);//因为函数里面写的是计算是X--，所以计算右边需要t2,t1
                    break;
            }
        }

        void SpawnCorriderElem(Transform t1, Transform t2, Transform groundParent, Transform wallParent, bool isX)
        {
            if (isX)
            {
                int x = (int)t1.position.x - 1;
                int targetX = (int)t2.position.x;
                int tempZ = (int)t1.transform.position.z;
                GameObject elem;
                for (; x > targetX; x--)//Left
                {
                    for (int z = tempZ - 3; z <= tempZ + 3; z++)
                    {
                        if (z >= tempZ - 2 && z <= tempZ + 2)
                        {
                            elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorGroundPath));
                            elem.transform.position = new Vector3(x, t1.transform.position.y, z);
                            elem.transform.SetParent(groundParent);
                        }
                        else
                        {
                            elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorWallPath));
                            elem.transform.position = new Vector3(x, t1.transform.position.y + 1, z);
                            elem.transform.SetParent(wallParent);
                        }
                    }
                }
            }
            else
            {
                int z = (int)t1.position.z - 1;
                int targetZ = (int)t2.position.z;
                int tempX = (int)t1.transform.position.x;

                GameObject elem;
                for (; z > targetZ; z--)//Down 
                {
                    for (int x = tempX - 3; x <= tempX + 3; x++)
                    {
                        if (x >= tempX - 2 && x <= tempX + 2)
                        {
                            elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorGroundPath));
                            elem.transform.position = new Vector3(x, t1.transform.position.y, z);
                            elem.transform.SetParent(groundParent);
                        }
                        else
                        {
                            elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorWallPath));
                            elem.transform.position = new Vector3(x, t1.transform.position.y + 1, z);
                            elem.transform.SetParent(wallParent);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 封闭地图块没有走廊的预留口
        /// </summary>
        void CloseCap(GameObject corridorPrefab, Transform mapParent, Transform corTrans, MapGrid check, bool isX)
        {
            if (check != null) return;
            GameObject corridor = Instantiate(corridorPrefab);
            corridor.transform.SetParent(mapParent);
            corridor.transform.position = corTrans.position;
            corridor.transform.rotation = corTrans.rotation;
            Transform wallP = corridor.transform.Find("Wall");
            GameObject elem;
            int tempI = isX ? (int)corTrans.position.x : (int)corTrans.position.z;
            for (int i = tempI - 2; i <= tempI + 2; i++)
            {
                elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorWallPath));
                elem.transform.SetParent(wallP);
                if (isX)
                {
                    elem.transform.position = new Vector3(i, corTrans.position.y + 1, corTrans.position.z);
                }
                else
                {
                    elem.transform.position = new Vector3(corTrans.position.x, corTrans.position.y + 1, i);
                }
            }
        }


    }//ClassEnd
}
