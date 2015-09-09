using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db.Models
{
    public enum MessageTypes : byte
    {
        /// <summary>
        /// 私信
        /// </summary>
        PrivateMail = 0,
        /// <summary>
        /// 评论
        /// </summary>
        Comment = 1,
        /// <summary>
        /// 被@了
        /// </summary>
        Mention = 2,
        /// <summary>
        /// 回复
        /// </summary>
        Reply = 3
    }
}
