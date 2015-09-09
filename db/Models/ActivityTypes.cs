using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db.Models
{
    public enum ActivityTypes : byte
    {
        /// <summary>
        /// 赞
        /// </summary>
        Praise = 0,
        /// <summary>
        /// 转存
        /// </summary>
        Resave = 1,
        /// <summary>
        /// 关注用户
        /// </summary>
        FollowUser = 2,
        /// <summary>
        /// 关注图包
        /// </summary>
        FollowPackage = 3,
        /// <summary>
        /// 通过其他图包转存
        /// </summary>
        ResaveThrough = 4
    }

}
