using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public class UIWindow2:Attribute
    {
        public string Layer;
    }

    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false)]
    public class InstAttribute : Attribute
    {
        public string path;
        public InstAttribute(string path) { this.path = path; }
    }
}
