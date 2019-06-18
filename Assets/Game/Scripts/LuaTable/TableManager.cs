using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public class TableManager
    {
        private readonly string[] tablesPath = new string[(int)TableType.Max] { "data/BulletTable", "data/CharacterModeInfoTable", "data/OccupationTable", "data/WeaponTable" };

        private Dictionary<TableType, DataTable> dicTables = new Dictionary<TableType, DataTable>();

        public void Init()
        {
            for (int i = 0; i < (int)TableType.Max; i++)
            {
                dicTables.Add((TableType)i, Game.LoadTable(tablesPath[i]));
            }
        }

        public DataTable GetTable(TableType tableType)
        {
            if (dicTables.ContainsKey(tableType))
            {
                return dicTables[tableType];
            }
            return null;
        }

        public T GetTableElement<T>(TableType tableType, int row, string key)
        {
            DataTable table = GetTable(tableType);
            return table.Get<T>(row, key);
        }
    }
}
