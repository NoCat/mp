using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db.Models
{
    public enum DownloadStates : byte
    {
        /// <summary>
        /// 未开始
        /// </summary>
        NotBegin = 0,
        /// <summary>
        /// 处理中
        /// </summary>
        Processing = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        Finished = 2
    }
}
