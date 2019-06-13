using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace YummyGame.Framework
{
    public class UIWindowMapping
    {
        private Dictionary<string, Dictionary<string, string>> m_instInject = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, List<string>> m_autocloseInject = new Dictionary<string, List<string>>();
        private Dictionary<string, Dictionary<string, string>> m_bind = new Dictionary<string, Dictionary<string, string>>();

        private List<YAttribute> m_handles = new List<YAttribute>();
        List<FieldInfo> auto_delay = new List<FieldInfo>();
        public void BuildWindow(Type windowType,UIWindow window)
        {
            if (m_instInject.ContainsKey(window.ResourcePath))
            {
                _dicInst(windowType, window);
            }
            else
            {
                _buildInst(windowType, window);
            }
        }

        public void DestroyWindow(Type windowType, UIWindow window)
        {
            FieldInfo[] fields = windowType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if ((field.FieldType != typeof(GameObject)) &&
                    !field.FieldType.IsSubclassOf(typeof(Component))) continue;
                object[] attrs = field.GetCustomAttributes(false);
                if (attrs == null || attrs.Length == 0) continue;
                foreach (var attr in attrs)
                {
                    if (attr is AutoCloseAttribute)
                    {
                        if (field.FieldType != typeof(Button)) continue;
                        Button btn = (Button)field.GetValue(window);
                        if (btn == null) return;
                        btn.onClick.RemoveAllListeners();
                    }
                    else if (attr is UIClickAttribute)
                    {
                        if (field.FieldType != typeof(Button)) continue;
                        Button btn = (Button)field.GetValue(window);
                        if (btn == null) return;
                        btn.onClick.RemoveAllListeners();
                    }
                }
            }
        }

        private void _dicInst(Type windowType,UIWindow window)
        {
            foreach (var info in m_instInject[window.ResourcePath])
            {
                FieldInfo field = windowType.GetField(info.Key);
                _inst(window, field, info.Value);
            }
            foreach (var info in m_autocloseInject[window.ResourcePath])
            {
                FieldInfo field = windowType.GetField(info);
                _autoClick(windowType,window, field);
            }
            foreach (var info in m_bind[window.ResourcePath])
            {
                FieldInfo field = windowType.GetField(info.Key);
                _bind(window, windowType,field, info.Value);
            }
        }

        private void _buildInst(Type windowType, UIWindow window)
        {
            m_instInject.Add(window.ResourcePath, new Dictionary<string, string>());
            m_autocloseInject.Add(window.ResourcePath, new List<string>());
            m_bind.Add(window.ResourcePath, new Dictionary<string, string>());
            FieldInfo[] fields = windowType.GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
            auto_delay.Clear();
            foreach (var field in fields)
            {
                if ((field.FieldType != typeof(GameObject)) &&
                    !field.FieldType.IsSubclassOf(typeof(Component))) continue;
                object[] attrs = field.GetCustomAttributes(false);
                m_handles.Clear();
                foreach (var attr in attrs)
                {
                    if (attr is YAttribute) m_handles.Add((YAttribute)attr);
                }
                m_handles.Sort((a, b) => a.sort - b.sort);
                if (attrs == null || attrs.Length == 0) continue;
                foreach(var attr in m_handles)
                {
                    if(attr is InstAttribute)
                    {
                        InstAttribute inst = attr as InstAttribute;
                        m_instInject[window.ResourcePath].Add(field.Name, inst.path);
                        _inst(window, field, inst.path);
                    }else if(attr is AutoCloseAttribute)
                    {
                        if (field.FieldType != typeof(Button)) continue;
                        auto_delay.Add(field);
                    }else if(attr is UIClickAttribute)
                    {
                        if (field.FieldType != typeof(Button)) continue;
                        UIClickAttribute click = attr as UIClickAttribute;
                        m_bind[window.ResourcePath].Add(field.Name, click.func);
                        _bind(window, windowType,field, click.func);
                    }
                }
                
            }

            foreach (var field in auto_delay)
            {
                m_autocloseInject[window.ResourcePath].Add(field.Name);
                _autoClick(windowType, window, field);
            }
        }

        private void _inst(UIWindow obj, FieldInfo field,string path)
        {
            Transform tran = obj.root.transform.Find(path);
            if (tran == null) return;
            if(field.FieldType == typeof(GameObject))
            {
                field.SetValue(obj, tran.gameObject);
            }
            else
            {
                field.SetValue(obj, tran.GetComponent(field.FieldType));
            }
        }

        private void _autoClick(Type windowType, UIWindow obj, FieldInfo field)
        {
            if (obj == null) return;
            MethodInfo method = windowType.GetMethod("Close",BindingFlags.NonPublic|BindingFlags.Instance);
            Button btn =(Button) field.GetValue(obj);
            if (btn == null) return;
            btn.onClick.AddListener(() => method.Invoke(obj, null));
        }

        private void _bind(UIWindow obj, Type objType, FieldInfo field, string func)
        {
            Button btn = (Button)field.GetValue(obj);
            MethodInfo method = objType.GetMethod(func, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null) return;
            if (btn == null) return;
            btn.onClick.AddListener(() => method.Invoke(obj, null));
        }
    }
}
