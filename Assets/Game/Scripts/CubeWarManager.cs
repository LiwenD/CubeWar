using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public class CubeWarManager : UnitySingleton<CubeWarManager>
    {
        TableManager tableManager;
        /// <summary>
        /// 游戏的lua表管理系统
        /// </summary>
        public TableManager MTableManager
        {
            get
            {
                if (tableManager == null)
                {
                    tableManager = new TableManager();
                    tableManager.Init();
                }
                return tableManager;
            }
        }
    }
}
