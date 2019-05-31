using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public interface IFactory<T>
    {
        T Get();
    }
}

