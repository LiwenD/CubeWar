using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public static class GameObjectExtension
    {
        public static T GetOrAddComponent<T>(this GameObject target)
            where T:Component
        {
            T t = target.GetComponent<T>();
            if (t == null)
            {
                t = target.AddComponent<T>();
            }
            return t;
        }
    }
}
