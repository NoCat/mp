using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quartz;

using mp.DAL;
using mp.BLL;

namespace Scheduler
{
    class UpdateImageWeightJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var db = new MiaopassContext();

            var sql = "update image set weight=weight*0.9";
            db.Database.ExecuteSqlCommand(sql);
        }
    }
}
