using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YummyGame.Framework
{
    public class BundleAsset:Asset
    {
        AssetBundle bundle;
        public BundleAsset(string path, Type type,Object obj,AssetBundle bundle) : base(path, type, obj)
        {
            this.bundle = bundle;
        }

        public override void Dispose()
        {
            if (asset == null || bundle == null) return;
            asset = null;
            bundle = null;
            
        }
    }
}
