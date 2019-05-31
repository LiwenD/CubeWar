using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public class TaskLooper : MonoBehaviour
    {
        private List<ITask> m_created;
        private List<ITask> m_tempCreated;
        private List<ITask> m_update;
        private Dictionary<ITask,Action> m_killed;
        private Dictionary<ITask, Action> m_Tempkilled;

        #region states
        bool _unsafeStart = false;
        bool _unsafeUpdate = false;
        bool _unsafeDelete = false;
        #endregion
        public void StartTask(ITask task)
        {
            if (m_created.Contains(task) || m_tempCreated.Contains(task)) return;
            if (!_unsafeStart)
                m_created.Add(task);
            else
                m_tempCreated.Add(task);
        }

        public void DebugTask()
        {
            Debug.Log("create:" + m_created.Count + m_update.Count);
            Debug.Log("update:" + m_update.Count);
            Debug.Log("kill:" + m_killed.Count + m_tempCreated.Count);
        }

        public void KillTask(ITask task,Action killFunc)
        {
            TaskState state = task.GetState();
            if (state == TaskState.Killed || m_killed.ContainsKey(task) || m_Tempkilled.ContainsKey(task)) return;
            if (_unsafeDelete)
                m_killed.Add(task, killFunc);
            else
                m_Tempkilled.Add(task, killFunc);
        }

        private void Awake()
        {
            m_created = new List<ITask>();
            m_tempCreated = new List<ITask>();
            m_update = new List<ITask>();
            m_killed = new Dictionary<ITask, Action>();
            m_Tempkilled = new Dictionary<ITask, Action>();
        }

        private void Update()
        {
            TaskStartState();
            TaskUpdateState();
            TaskKillState();
        }

        private void TaskStartState()
        {
            _unsafeStart = true;
            int index = 0;
            do
            {
                m_tempCreated.Clear();
                for (; index < m_created.Count; index++)
                {
                    m_created[index].onLooperStart();
                    if(!m_killed.ContainsKey(m_created[index]))
                        m_update.Add(m_created[index]);

                }
                for (int i = 0; i < m_tempCreated.Count; i++)
                {
                    m_created.Add(m_tempCreated[i]);
                }

            } while (m_tempCreated.Count > 0);
            _unsafeStart = false;
            m_created.Clear();
        }

        private void TaskUpdateState()
        {
            _unsafeUpdate = true;
            for (int i = 0; i < m_update.Count; i++)
            {
                var task = m_update[i];
                if (!task.onLooperUpdate())
                {
                    m_update.RemoveAt(i);
                    i--;
                    task.onLooperEnd();
                }
            }
            _unsafeUpdate = false;
        }

        private void TaskKillState()
        {
            int index = 0;
            _unsafeDelete = true;
            do
            {
                foreach (var kv in m_killed)
                {
                    index++;
                    if (m_update.Contains(kv.Key))
                    {
                        m_update.Remove(kv.Key);
                        kv.Key.onLooperEnd();
                    }
                    kv.Value();
                }
                m_killed.Clear();
                foreach (var kv in m_Tempkilled)
                {
                    m_killed.Add(kv.Key, kv.Value);
                }
                m_Tempkilled.Clear();
            } while (m_killed.Count>0);
            
            _unsafeDelete = false;
            
        }

        private void OnDisable()
        {
            m_created.ForEach((task) => task.Kill());
            m_update.ForEach((task) => task.Kill());

            TaskKillState();
        }
    }
}
