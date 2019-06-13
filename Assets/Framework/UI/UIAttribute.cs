using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public class YAttribute:Attribute
    {
        public int sort = 0;
    }
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false)]
    public class InstAttribute : YAttribute
    {
        public string path;
        public InstAttribute(string path)
        {
            sort = 0;
            this.path = path;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AutoCloseAttribute : YAttribute
    {
        public AutoCloseAttribute() { sort = 1; }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class UIClickAttribute : YAttribute
    {
        public string func;
        public UIClickAttribute(string func)
        {
            sort = 2;
            this.func = func;
        }
    }
}
