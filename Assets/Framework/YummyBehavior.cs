using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class YummyBehavior:MonoBehaviour
    {
        protected AssetLoader loader;
        protected virtual void Awake()
        {
            loader = new AssetLoader();
        }

        protected virtual void OnDestroy()
        {
            loader?.Destroy();
        }
    }
}
