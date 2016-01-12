using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

namespace mp.BLL
{
    public class ResaveChainManager : ManagerBase<ResaveChain>
    {
        public ResaveChainManager(MiaopassContext db, ManagerCollection colletion) : base(db, colletion) { }

        public override ResaveChain Add(ResaveChain entity, bool save = true)
        {
            var addList = DB.ResaveChains.Where(r => r.Child == entity.Parent)
                .Select(r => new ResaveChain { Parent = r.Parent, Child = entity.Child, PathLength = r.PathLength + 1 })
                .ToList();
            addList.Add(entity);
            DB.ResaveChains.AddRange(addList);
            DB.SaveChanges();
            return entity;
        }
    }
}
