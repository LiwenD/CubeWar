using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace YummyGame.Framework
{
    public enum UIOpenType
    {
        Open,
        MultiOpen
    }
    public class UIManager:UnitySingleton<UIManager>
    {
        
        private Dictionary<string, Dictionary<long,UIWindow>> m_layersWindow = new Dictionary<string, Dictionary<long,UIWindow>>();
        private Dictionary<long, UIWindow> m_creates = new Dictionary<long, UIWindow>();
        private Dictionary<long, UIWindow> m_shows = new Dictionary<long, UIWindow>();
        private Dictionary<Type, List<UIWindow>> m_caches = new Dictionary<Type, List<UIWindow>>();
        private Dictionary<string, Transform> m_layers = new Dictionary<string, Transform>();
        private Dictionary<string, ITask> m_tasks = new Dictionary<string, ITask>();



        private Transform m_root;

        private Canvas m_canvas;
        private float ratio;
        private Vector2 canvasSize;
        private long _uuid;

        private UIWindowMapping _mapping;
        private void Awake()
        {
            _mapping = new UIWindowMapping();
            if (!_initRoot()) return;
            _initLayers();
        }

        public void CloseUI(long uuid, bool destroy = false)
        {
            if (!m_shows.ContainsKey(uuid)) return;
            CloseUI(m_shows[uuid],destroy);
        }

        public void CloseUI(UIWindow window, bool destroy = false)
        {
            if (!m_layersWindow[window.layer].ContainsKey(window.uuid)) return;
            _remShowDic(window);
            if (destroy)
                DestroyUI(window);
            else
            {
                Type uiWindowType = window.GetType();
                if (!m_caches.ContainsKey(uiWindowType))
                {
                    m_caches.Add(uiWindowType, new List<UIWindow>());
                }
                m_caches[uiWindowType].Add(window);
            }
        }

        private void DestroyUI(UIWindow window)
        {
            GameObject.Destroy(window.root);
            window.loader.Destroy();
        }

        public YummyTask<long> OpenUI<T>(string layer)where T : UIWindow
        {
            Type windowType = typeof(T);
            UIWindow window = _getFromCaches(windowType);
            return _CreateUI(windowType, window, layer);
        }

        private YummyTask<long> _CreateUI(Type windowType,UIWindow window,string layer)
        {
            window.uuid = ++_uuid;
            window.layer = layer;
            m_creates.Add(window.uuid, window);
            window.OnCreate();
            return _OpenUI(windowType, window, layer).As(() => window.uuid);
        }

        private ITask _OpenUI(Type windowType,UIWindow window, string layer)
        {
            var task = UIWindowTask.OpenUI(window);
            task.OnTaskFinish((windowGo) =>
            {
                window.root = windowGo;
                _mapping.BuildWindow(windowType, window);

                _addShowDic(window, layer);
                window.OnShow();
            });
            return task;
        }

        private UIWindow _getFromCaches(Type windowType)
        {
            if(!m_caches.ContainsKey(windowType)||m_caches[windowType].Count == 0)
               return (UIWindow)Activator.CreateInstance(windowType);
            UIWindow window = m_caches[windowType][0];
            m_caches[windowType].RemoveAt(0);
            return window;
        }

        private void _remShowDic(UIWindow window)
        {
            window.OnHide();
            window.root.SetActive(false);
            m_shows.Remove(window.uuid);
            m_layersWindow[window.layer].Remove(window.uuid);
        }

        private void _addShowDic(UIWindow window,string layer)
        {
            window.root.transform.SetParent(GetLayer(layer), false);
            window.root.transform.SetAsLastSibling();

            m_shows.Add(window.uuid, window);
            m_layersWindow[layer].Add(window.uuid, window);
        }

        public Transform GetLayer(string layer)
        {
            if (m_layers.ContainsKey(layer)) return m_layers[layer];
            return null;
        }

        private bool _initRoot()
        {
            GameObject root = GameObject.Find("Canvas");
            if (root == null) return false;
            Object.DontDestroyOnLoad(root);
            m_root = root.transform;
            m_canvas = m_root.GetComponent<Canvas>();
            var scaler = m_root.GetComponent<CanvasScaler>();
            var resolution = scaler.referenceResolution;
            var match = scaler.matchWidthOrHeight;

            ratio = Screen.width / resolution.x * (1 - match) + Screen.height / resolution.y * match;
            canvasSize = new Vector2(Screen.width / ratio, Screen.height / ratio);

            Canvas.ForceUpdateCanvases();
            return true;
        }

        private void _initLayers()
        {
            var layersTran = m_root.Find("Layers");
            foreach (Transform layer in layersTran)
            {
                m_layers.Add(layer.name, layer);
                m_layersWindow.Add(layer.name, new Dictionary<long, UIWindow>());
            }
        }
    }
}
