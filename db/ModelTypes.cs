using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp
{
    public enum ModelTypes : byte
    {
        /// <summary>
        /// 转存图片
        /// </summary>
        Resave = 0,
        /// <summary>
        /// 创建图片
        /// </summary>
        Add = 1,
        /// <summary>
        /// 编辑图片
        /// </summary>
        Edit=2
    }
}
