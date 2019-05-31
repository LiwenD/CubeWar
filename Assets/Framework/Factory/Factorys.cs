using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyGame.Framework
{
    public class Factorys<T> : IFactory<T>
    {
        private Func<T> allocFunc;
        public Factorys(Func<T> allocFunc)
        {
            this.allocFunc = allocFunc;
        }
        public T Get()
        {
            return allocFunc();
        }
    }
}
