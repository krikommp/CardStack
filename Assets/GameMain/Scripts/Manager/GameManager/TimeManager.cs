using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeScheduledHandle = System.Int32;

namespace Client.GameLogic.Manager.GameManager
{
    [Serializable]
    public class ScheduledTask
    {
        public DateTime m_triggerTime;
        public Action m_action;
        public TimeSpan? m_loopInterval;
        
        public ScheduledTask(DateTime triggerTime, Action action, TimeSpan? loopInterval = null)
        {
            m_triggerTime = triggerTime;
            m_action = action;
            m_loopInterval = loopInterval;
        }
    }

    public class TimeManager : IManagerInitializable, IManagerUpdate
    {
        #region Implementation of IManagerInitializable

        public IEnumerator ManagerInit()
        {
            yield return null;
            
            Debug.Log("[TimeManager] 时间管理器初始化完成");
        }

        public int Priority { get; } = (int)EManagerPriority.Low;

        #endregion


        private float m_secondsPerGameDay = 1.0f;
        private float m_gameSpeed = 1.0f;
        private bool m_bPaused = false;
        private DateTime m_currentDateTime = DateTime.Today;
        public const TimeScheduledHandle kInvalidTimeScheduledHandle = -1;

        private List<ScheduledTask> m_scheduledTasks = new();
        #region Implementation of IManagerUpdate

        public void Update()
        {
            if (!m_bPaused)
            {
                float deltaGameTime  = Time.deltaTime * m_gameSpeed / m_secondsPerGameDay;
                m_currentDateTime = m_currentDateTime.AddDays(deltaGameTime);
                CheckScheduledTasks();
            }
        }

        #endregion
        
        private void CheckScheduledTasks()
        {
            for (int i = m_scheduledTasks.Count - 1; i >= 0; i--)
            {
                var task = m_scheduledTasks[i];

                if (task.m_triggerTime <= m_currentDateTime)
                {
                    if (task.m_loopInterval.HasValue)
                    {
                        task.m_triggerTime = task.m_triggerTime.Add(task.m_loopInterval.Value);
                    }
                    else
                    {
                        m_scheduledTasks.RemoveAt(i);
                    }
                    
                    task.m_action?.Invoke();
                }
            }
        }

        public TimeScheduledHandle ScheduleTaskAddDay(int days, Action action, bool isLoop, out DateTime endTine)
        {
            TimeSpan? timeSpan = null;
            if (isLoop)
            {
                timeSpan = TimeSpan.FromDays(days);
            }

            var triggerTime = m_currentDateTime.AddDays(days);

            m_scheduledTasks.Add(new ScheduledTask(m_currentDateTime.AddDays(days), action, timeSpan));

            endTine = triggerTime;
            return (TimeScheduledHandle)(m_scheduledTasks.Count - 1);
        }

        public TimeScheduledHandle ScheduleTaskAddMouth(float mouth, Action action, bool isLoop, out DateTime endTine)
        {
            float days = 30 * mouth;
            TimeSpan? timeSpan = null;
            if (isLoop)
            {
                timeSpan = TimeSpan.FromDays(days);
            }
            
            var triggerTime = m_currentDateTime.AddDays(days);
            
            m_scheduledTasks.Add(new ScheduledTask(m_currentDateTime.AddDays(days), action, timeSpan));

            endTine = triggerTime;
            return (TimeScheduledHandle)(m_scheduledTasks.Count - 1);
        }
        
        public bool CancelScheduledTask(TimeScheduledHandle handle)
        {
            if (handle == kInvalidTimeScheduledHandle)
            {
                return false;
            }

            if (handle < 0 || handle >= m_scheduledTasks.Count)
            {
                return false;
            }

            m_scheduledTasks.RemoveAt(handle);
            return true;
        }

        public DateTime GetGameDateTime()
        {
            return m_currentDateTime;
        }
    }
}