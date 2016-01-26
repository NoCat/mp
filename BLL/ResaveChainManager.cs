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
            //获取所有的父节点   
            var addList = new List<ResaveChain>();
            DB.ResaveChains.Where(r => r.Child == entity.Parent)
                .ToList()
                .ForEach(r =>
                {
                    addList.Add(new ResaveChain { Parent = r.Parent, Child = entity.Child, PathLength = r.PathLength + 1 });
                });
            addList.Add(entity);

            //所有父节点的resaveCount加1
            var imageIds = addList.Select(i => i.Parent);
            var images = DB.Images.Where(i => imageIds.Contains(i.ID)).ToList();
            images.ForEach(i => { i.ResaveCount++; });

            DB.Transaction(() =>
            {
                //添加新的chain
                var sql1 = string.Format("insert into resavechain (parent,child,pathlength) select parent,child,pathlength+1 from resavechain where child=? union select ?,?,0",entity.Parent,entity.Parent,entity.Child);
                DB.Database.ExecuteSqlCommand(sql1, entity.Parent, entity.Parent, entity.Child);

                //更新相关图片信息
                var sql2 = string.Format("update image set praisecount=praisecount+1 where id in (select parent from resavechain where child=?)");
                DB.Database.ExecuteSqlCommand(sql2,entity.Child);
                //DB.UpdateRange(images, false);

                //保存所有的chains
                DB.AddRange(addList, false);
                DB.SaveChanges();
            });

            return entity;
        }
    }
}
