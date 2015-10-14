using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mp.DAL;

namespace mp.BLL
{
    public class PickManager:ManagerBase<Pick>
    {
        public PickManager(MiaopassContext context) : base(context) { }
    }
}
