using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db.Models
{
    public enum FollowingTypes : byte
    {
        /// <summary>
        /// 用户
        /// </summary>
        User = 0,
        /// <summary>
        /// 图包
        /// </summary>
        Package = 1
    }
}
