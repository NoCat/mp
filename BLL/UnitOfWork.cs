using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mp.DAL;

namespace mp.BLL
{
    public class UnitOfWork
    {
        MiaopassContext _context = new MiaopassContext();
        ManagerBase<Url> _urls;
        ManagerBase<Pick> _picks;

        public ManagerBase<Url> Urls
        {
            get
            {
                if (_urls == null)
                    _urls = new ManagerBase<Url>(_context);
                return _urls;
            }
        }
        public ManagerBase<Pick> Picks
        {
            get
            {
                if (_picks == null)
                    _picks = new ManagerBase<Pick>(_context);
                return _picks;
            }
        }


        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
