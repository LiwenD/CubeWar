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
        public GameObject mapGo;     //该格子代表的地图物体

        public IndexInfo indexInfo=new IndexInfo();  //该格子在二维数组中的列数、行数
        public GridType gridType=GridType.None;

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
    }
}
