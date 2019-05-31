using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public class Bundle : Asset
    {
        public AssetBundle bundle { get; internal set; }

        public Bundle(string path) : this(path, null, 0) { }

        public override bool IsValid => base.IsValid && bundle != null;

        public Bundle(string path,AssetBundle bundle,int size) 
            : base(path, typeof(AssetBundle), bundle)
        {
            this.size = size;
            this.bundle = bundle;
        }

        public new Bundle Get()
        {
            referenceCount++;
            return this;
        }


        public override void Dispose()
        {
            if (bundle == null) return;
            bundle.Unload(false);
            asset = null;
            bundle = null;
        }
    }
}
