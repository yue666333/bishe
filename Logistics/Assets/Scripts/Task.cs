using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Task
    {
        //任务名
        private string m_TaskName;
        public string TaskName
        {
            set
            {
                m_TaskName = value;
            }
            get
            {
                return m_TaskName;
            }
        }

        //任务具体内容，外部传入
        public Action Work;

        public Task(Action work, string taskName = "defaultTaskName")
        {
            this.Work = work;
            this.m_TaskName = taskName;
        }
    }
}
