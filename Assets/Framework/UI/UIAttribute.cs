using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false)]
    public class InstAttribute : Attribute
    {
        public string path;
        public InstAttribute(string path) { this.path = path; }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AutoCloseAttribute : Attribute
    {

    }
}
