using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public abstract class UIWindow
    {
        public long uuid;
        public string layer;
        public readonly AssetLoader loader = new AssetLoader();
        public GameObject root;
        public object args;
        public virtual void OnCreate() { }
        public virtual void OnDestroy() { }
        public virtual void OnShow() { }
        public virtual void OnHide() { }
        public abstract string ResourcePath { get; }
        protected void Close()
        {
            UIManager.Instance.CloseUI(uuid);
        }
        protected void Open<T>(string layer)where T:UIWindow
        {
            UIManager.Instance.OpenUI<T>(layer);
        }
    }

}
