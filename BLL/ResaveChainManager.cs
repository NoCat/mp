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

            DB.Transaction(() =>
            {
                //所有父节点的resaveCount加1
                DB.Images.Where(i => addList.Select(a => a.Parent).Contains(i.ID)).Update(i => new Image { ResaveCount = i.ResaveCount + 1 });

                //保存所有的chains
                DB.ResaveChains.AddRange(addList);
                DB.SaveChanges();
            });
            return entity;
        }
    }
}
