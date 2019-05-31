using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyGame.Framework
{
    public class NullObject
    {
        public static NullObject Default = new NullObject();
        private NullObject()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is NullObject) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return "null";
        }
    }
}
