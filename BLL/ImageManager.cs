using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mp.DAL;
using System.Transactions;

namespace mp.BLL
{
    public class ImageManager:ManagerBase<Image>
    {
        public ImageManager(MiaopassContext context) : base(context) { }

        public override void Insert(Image entity, bool save = true)
        {
            using(var transaction=new TransactionScope())
            {
                Context.Insert(entity);
                Service.Packages.Update(new Package { ID = entity.PackageID, LastModify = entity.CreatedTime });
                transaction.Complete();
            }
        }
    }
}
