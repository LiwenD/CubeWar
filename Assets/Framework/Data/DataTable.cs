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
#if XLUA_SUPPORT
        public LuaTable GetRowTable(int row)
        {

            LuaTable rowTable = null;
            _internalTable.Get(row, out rowTable);
            return rowTable;
        }
#endif

        public IEnumerable<int> GetAllRows()
        {
#if XLUA_SUPPORT
            return _internalTable.GetKeys<int>();
#else
            return null;
#endif
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
