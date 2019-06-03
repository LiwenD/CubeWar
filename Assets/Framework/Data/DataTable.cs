using System;
using System.Collections.Generic;

#if XLUA_SUPPORT
using XLua;
#endif

namespace YummyGame.Framework
{
    public class DataTable
    {
#if XLUA_SUPPORT
        public LuaTable _internalTable;

#endif
        public LuaTable GetRowTable(int row)
        {
#if XLUA_SUPPORT
            LuaTable rowTable = null;
            _internalTable.Get(row, out rowTable);
            return rowTable;
#else
            return null;
#endif
        }

        public IEnumerable<int> GetAllRows()
        {
            return _internalTable.GetKeys<int>();
        }

        public T Get<T>(int row,string key)
        {
#if XLUA_SUPPORT
            LuaTable table = GetRowTable(row);
            if (table == null) return default(T);
            return table.Get<T>(key);
#else
            return default(T);
#endif
        }

    }
}
