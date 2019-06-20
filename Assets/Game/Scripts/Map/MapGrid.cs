using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.CubeWar
{
    public class MapGrid
    {
        public GameObject magGo;     //该格子代表的地图物体


        /*
         * 这个地图格子的上下左右的邻居格子的信息
         * */
        public MapGrid Up { get; set; }
        public MapGrid Down { get; set; }
        public MapGrid Left { get; set; }
        public MapGrid Right { get; set; }
    }
}
