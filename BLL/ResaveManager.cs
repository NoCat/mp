using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntityFramework.Extensions;

using mp.DAL;

namespace mp.BLL
{
    public class ResaveManager : ManagerBase<Resave>
    {
        public ResaveManager(MiaopassContext db, ManagerCollection colletion) : base(db, colletion) { }

        public override Resave Add(Resave entity, bool save = true)
        {
            DB.Transaction(() =>
            {
                //添加新的chain
                var parents = DB.Resaves
                    .Where(r => r.Child == entity.Parent)
                    .ToList();
                var resaves = new List<Resave>();
                foreach (var item in parents)
                {
                    resaves.Add(new Resave
                    {
                        Parent = item.Parent,
                        Child = entity.Child,
                        CreateTime = entity.CreateTime,
                        PathLength = item.PathLength + 1
                    });
                }
                resaves.Add(entity);
                DB.AddRange(resaves);

                //更新相关图片信息
                var images = (from image in DB.Images
                             join resave in DB.Resaves on image.ID equals resave.Parent
                             where resave.Child==entity.Child
                             select image).ToList();
                images.ForEach(i => i.ResaveCount++);
                DB.UpdateRange(images);

                //被转存的图片权重更新                
                var child=DB.Images.Find(entity.Child);
                var exist = DB.Images.Where(i => i.UserID == child.UserID && i.FileID == child.FileID).Count() > 0;
                if (exist == false)
                {
                    var parent = DB.Images.Find(entity.Parent);
                    parent.Weight += 10000;
                    DB.Update(parent);
                }
            });

            return entity;
        }
    }
}
