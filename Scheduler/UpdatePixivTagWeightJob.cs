using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mp.BLL;
using mp.DAL;

namespace Scheduler
{
    class UpdatePixivTagWeightJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var db = new MiaopassContext();
            var sql = @"update adminpixivtag set weight=weight*0.9";
            db.Database.ExecuteSqlCommand(sql);
        }
    }
}
