using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using mp.DAL;

namespace mp.BLL
{
    public class AdminPixivTagManager:ManagerBase<AdminPixivTag>
    {
        public AdminPixivTagManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }

        public override AdminPixivTag Add(AdminPixivTag entity, bool save = true)
        {
            //如果为全英文,则自动将mtext=ptext;
            if(Regex.IsMatch(entity.PText,@"^[a-zA-Z]+$"))
            {
                entity.MText = entity.PText;
            }

            //如果含有"users入り",则直接设置为跳过
            if (entity.PText.Contains("users入り"))
            {
                entity.IsSkip = true;
            }

            return base.Add(entity, save);
        }
    }
}
