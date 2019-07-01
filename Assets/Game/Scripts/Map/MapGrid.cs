using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.CubeWar
{
    /// <summary>
    /// 该格子在二维数组中的列数、行数
    /// </summary>
    public struct IndexInfo
    {
        public int x;
        public int y;
    }

    public class MapGrid
    {
        private GameObject mapGo;
        public GameObject MapGo { get { return MapGo; } }     //该格子代表的地图物体

        public Transform UpTab { private set; get; }      //tab记录的是地图物体上下左右开口的位置
        public Transform DownTab { private set; get; }
        public Transform LeftTab { private set; get; }
        public Transform RightTab { private set; get; }

        public IndexInfo indexInfo = new IndexInfo();  //该格子在二维数组中的列数、行数
        public GridType gridType = GridType.None;

        bool isInited = false;
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        public bool IsInited { get { return isInited; } set { isInited = value; } }

        /*
         * 这个地图格子的上下左右的邻居格子的信息
         * */
        public MapGrid Up { get; set; }
        public MapGrid Down { get; set; }
        public MapGrid Left { get; set; }
        public MapGrid Right { get; set; }

        /// <summary>
        /// 设置地图物体
        /// </summary>
        /// <param name="go"></param>
        public void SetMapGo(GameObject go)
        {
            mapGo = go;

            UpTab = mapGo.transform.Find("Dir/Up");
            DownTab = mapGo.transform.Find("Dir/Down");
            LeftTab = mapGo.transform.Find("Dir/Left");
            RightTab = mapGo.transform.Find("Dir/Right");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static bool CheckCorridor(MapGrid grid)
        {
            if (grid != null && !grid.IsInited)
            {
                return true;
            }
            return false;
        }
    }
}
