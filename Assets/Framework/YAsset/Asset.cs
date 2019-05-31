using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YummyGame.Framework
{
    public class Asset:IDisposable
    {
        public Object asset;
        public Type assetType;
        public int referenceCount;
        public int size;
        private string path;
        private List<Asset> m_dependencies;

        public virtual bool IsValid
        {
            get { return asset != null; }
        }

        public Asset(string path,Type type) : this(path, type, null) { }

        public Asset(string path,Type type,Object obj)
        {
            this.path = path;
            this.assetType = type;
            this.asset = obj;
            m_dependencies = new List<Asset>();
        }

        public Asset Get()
        {
            m_dependencies.ForEach((asset) => asset.Get());
            referenceCount++;
            return this;
        }

        public void AddDependence(Asset asset)
        {
            if(!m_dependencies.Contains(asset))
                m_dependencies.Add(asset);
        }

        public void Release()
        {
            m_dependencies.ForEach((asset) => asset.Release());
            referenceCount--;
            if (referenceCount == 0)
            {
                Dispose();
            }
        }

        public virtual void Dispose()
        {
            if (asset == null) return;
            if (assetType != typeof(GameObject) 
                && assetType!=typeof(Component)
                && assetType != typeof(AssetBundle))
            {
                Resources.UnloadAsset(asset);
            }  
            asset = null;
        }
    }
}
