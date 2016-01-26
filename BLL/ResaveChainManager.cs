using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntityFramework.Extensions;

using mp.DAL;

namespace mp.BLL
{
    public class ResaveChainManager : ManagerBase<ResaveChain>
    {
        public ResaveChainManager(MiaopassContext db, ManagerCollection colletion) : base(db, colletion) { }

        public override ResaveChain Add(ResaveChain entity, bool save = true)
        {
            DB.Transaction(() =>
            {
                //添加新的chain
                var sql1 = string.Format("insert into resavechain (parent,child,pathlength) select parent,?,pathlength+1 from resavechain where child=? union select ?,?,0",entity.Parent,entity.Parent,entity.Child);
                DB.Database.ExecuteSqlCommand(sql1,entity.Child, entity.Parent, entity.Parent, entity.Child);

                //更新相关图片信息
                var sql2 = string.Format("update image set resavecount=resavecount+1 where id in (select parent from resavechain where child=?)");
                DB.Database.ExecuteSqlCommand(sql2,entity.Child);
            });

            return entity;
        }
    }
}
