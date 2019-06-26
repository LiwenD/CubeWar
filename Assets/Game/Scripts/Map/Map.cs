using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public class Map : MonoBehaviour
    {
        public void SpawnCorridor(MapGrid mapGrid, Transform mapParent)
        {
            AssetLoader assetLoader = new AssetLoader();
            GameObject corridorPrefab = assetLoader.LoadAsset<GameObject>("Map/CorridorPrefab");//todo 换常量
            if (MapGrid.CheckCorridor(mapGrid.Up))
            {
                InstantiateCorridor(corridorPrefab, mapParent, new Vector3(0, 0, -4));
            }

            if (MapGrid.CheckCorridor(mapGrid.Down))
            {
                InstantiateCorridor(corridorPrefab, mapParent, new Vector3(0, 0, 4));
            }

            if (MapGrid.CheckCorridor(mapGrid.Left))
            {
                InstantiateCorridor(corridorPrefab, mapParent, new Vector3(-4, 0, 0), true);
            }

            if (MapGrid.CheckCorridor(mapGrid.Right))
            {
                InstantiateCorridor(corridorPrefab, mapParent, new Vector3(4, 0, 0), true);
            }

        }

        void InstantiateCorridor(GameObject corridorPrefab, Transform par, Vector3 v3, bool isRotate90 = false)
        {
            GameObject corridor = Instantiate(corridorPrefab);
            corridor.transform.SetParent(par);
            corridor.transform.localPosition =  transform.localPosition + v3;
            if (isRotate90)
            {
                corridor.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }
}
