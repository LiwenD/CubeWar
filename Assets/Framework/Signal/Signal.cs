using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyGame.Framework
{
    
    public class Signal0
    {
        private Dictionary<Delegate, bool> m_actions = new Dictionary<Delegate, bool>();

        private List<Delegate> m_remove = new List<Delegate>();
        public void Add(Action action)
        {
            
            if (!m_actions.ContainsKey((Delegate)action))
            {
                m_actions.Add(action, false);
            }
            else
            {
                throw new ArgumentException("add error");
            }
        }

        public void AddOnce(Action action)
        {
            if (!m_actions.ContainsKey((Delegate)action))
            {
                m_actions.Add(action, true);
            }
            else
            {
                throw new ArgumentException("add error");
            }
        }

        public void Dispatch()
        {
            foreach (var kv in m_actions)
            {
                var action = (Action)kv.Key;
                action?.Invoke();
                if (kv.Value == true) m_remove.Add(kv.Key);
            }
            m_remove.Clear();
        }
    }

    public class Signal1<T>
    {
        private Dictionary<Delegate, bool> m_actions = new Dictionary<Delegate, bool>();

        private List<Delegate> m_remove = new List<Delegate>();
        public void Add(Action<T> action)
        {

            if (!m_actions.ContainsKey((Delegate)action))
            {
                m_actions.Add(action, false);
            }
            else
            {
                throw new ArgumentException("add error");
            }
        }

        public void AddOnce(Action<T> action)
        {
            if (!m_actions.ContainsKey((Delegate)action))
            {
                m_actions.Add(action, true);
            }
            else
            {
                throw new ArgumentException("add error");
            }
        }

        public void Dispatch(T args)
        {
            foreach (var kv in m_actions)
            {
                var action = (Action<T>)kv.Key;
                action?.Invoke(args);
                if (kv.Value == true) m_remove.Add(kv.Key);
            }
            m_remove.Clear();
        }
    }

    public class Signal2<T1,T2>
    {
        private Dictionary<Delegate, bool> m_actions = new Dictionary<Delegate, bool>();

        private List<Delegate> m_remove = new List<Delegate>();
        public void Add(Action<T1,T2> action)
        {

            if (!m_actions.ContainsKey((Delegate)action))
            {
                m_actions.Add(action, false);
            }
            else
            {
                throw new ArgumentException("add error");
            }
        }

        public void AddOnce(Action<T1, T2> action)
        {
            if (!m_actions.ContainsKey((Delegate)action))
            {
                m_actions.Add(action, true);
            }
            else
            {
                throw new ArgumentException("add error");
            }
        }

        public void Dispatch(T1 args1,T2 args2)
        {
            foreach (var kv in m_actions)
            {
                var action = (Action<T1,T2>)kv.Key;
                action?.Invoke(args1,args2);
                if (kv.Value == true) m_remove.Add(kv.Key);
            }
            m_remove.Clear();
        }
    }

}
