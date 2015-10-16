using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mp.Utility
{
    public class Job
    {
        public Worker Parent { get; set; }
        /// <summary>
        /// 构造函数,默认执行一次，除非指定interval为非0值
        /// </summary>
        public Job()
        {
            Interval = TimeSpan.Zero;
            LastExcute = DateTime.MinValue;
        }
        public object Parameter { get; set; }
        public DateTime LastExcute { set; get; }
        public bool IsProcessing { set; get; }
        public TimeSpan Interval { set; get; }
        virtual protected void ExcuteCore(object param) { }
        public void Excute()
        {
            IsProcessing = true;
            ExcuteCore(Parameter);
            LastExcute = DateTime.Now;
            IsProcessing = false;
            if (Interval == TimeSpan.Zero)
            {
                Parent.Jobs.Remove(this);
            }
        }
    }
}