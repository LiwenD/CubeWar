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
            Vector3 pos;
            //Debug.Log(assetLoader.LoadAsset<GameObject>(Consts.CorridorGroundPath));
            //Debug.Log(assetLoader.LoadAsset<GameObject>(Consts.CorridorWallPath));
            //return;

            if (MapGrid.CheckCorridor(mapGrid.Up))
            {
                pos = V3ExpandFunc.GetMidPoint(mapGrid.MapGo.transform.localPosition, mapGrid.Up.MapGo.transform.localPosition);
                InstantiateCorridor(corridorPrefab, mapParent, pos, mapGrid, CorridorDirectionm.Up);
            }

            if (MapGrid.CheckCorridor(mapGrid.Down))
            {
                pos = V3ExpandFunc.GetMidPoint(mapGrid.MapGo.transform.localPosition, mapGrid.Down.MapGo.transform.localPosition);
                InstantiateCorridor(corridorPrefab, mapParent, pos, mapGrid, CorridorDirectionm.Down);
            }

            if (MapGrid.CheckCorridor(mapGrid.Left))
            {
                pos = V3ExpandFunc.GetMidPoint(mapGrid.MapGo.transform.localPosition, mapGrid.Left.MapGo.transform.localPosition);
                InstantiateCorridor(corridorPrefab, mapParent, pos, mapGrid, CorridorDirectionm.Left);
            }

            if (MapGrid.CheckCorridor(mapGrid.Right))
            {
                pos = V3ExpandFunc.GetMidPoint(mapGrid.MapGo.transform.localPosition, mapGrid.Right.MapGo.transform.localPosition);
                InstantiateCorridor(corridorPrefab, mapParent, pos, mapGrid, CorridorDirectionm.Right);
            }
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
                    //Debug.Log(mapGrid.MapGo.transform.localPosition);
                    //Debug.Log(mapGrid.Right.MapGo.transform.localPosition);
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
                for (; x > targetX; x--)//Left
                {
                    GameObject elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorGroundPath));
                    elem.transform.position = new Vector3(x, t1.transform.position.y, t1.transform.position.z);
                    elem.transform.SetParent(groundParent);
                    for (int z = tempZ - 2; z <= tempZ + 2; z++)
                    {
                        if (z == tempZ) continue;
                        elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorWallPath));
                        elem.transform.position = new Vector3(x, t1.transform.position.y, z);
                        elem.transform.SetParent(wallParent);
                    }
                }
            }
            else
            {
                int z = (int)t1.position.z - 1;
                int targetZ = (int)t2.position.z;
                int tempX = (int)t1.transform.position.x;
                for (; z > targetZ; z--)//Down 
                {
                    GameObject elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorGroundPath));
                    elem.transform.position = new Vector3(t1.transform.position.x, t1.transform.position.y, z);
                    elem.transform.SetParent(groundParent);
                    for (int x = tempX - 2; x <= tempX + 2; x++)
                    {
                        if (x == tempX) continue;
                        elem = Instantiate(assetLoader.LoadAsset<GameObject>(Consts.CorridorWallPath));
                        elem.transform.position = new Vector3(x, t1.transform.position.y, z);
                        elem.transform.SetParent(wallParent);
                    }
                }
            }
        }
    }
}
