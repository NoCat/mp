using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mp.DAL;

namespace mp.BLL
{
    public class ManagementService:IDisposable
    {
        MiaopassContext _context;
        ManagerBase<Url> _urls;
        ManagerBase<Pick> _picks;
        ManagerBase<Image> _images;
        ManagerBase<Package> _packages;
        ManagerBase<User> _users;
        ManagerBase<Praise> _praises;
        ManagerBase<Following> _followings;
        ManagerBase<Download> _downloads;

        public ManagerBase<Download> Downloads
        {
            get
            {
                if (_downloads == null)
                    _downloads = new ManagerBase<Download>(_context,this);
                return _downloads;
            }
        }

        public ManagerBase<Following> Followings
        {
            get
            {
                if (_followings == null)
                    _followings = new ManagerBase<Following>(_context,this);
                return _followings;
            }
        }

        public ManagerBase<Praise> Praises
        {
            get
            {
                if (_praises == null)
                    _praises = new ManagerBase<Praise>(_context,this);
                return _praises;
            }
        }

        public ManagerBase<User> Users
        {
            get
            {
                if (_users == null)
                    _users = new ManagerBase<User>(_context,this);
                return _users;
            }
        }

        public ManagerBase<Package> Packages
        {
            get
            {
                if (_packages == null)
                    _packages = new ManagerBase<Package>(_context,this);
                return _packages;
            }
        }

        public ManagerBase<Image> Images
        {
            get
            {
                if (_images == null)
                    _images = new ImageManager(_context,this);
                return _images;
            }
        }

        public ManagerBase<Url> Urls
        {
            get
            {
                if (_urls == null)
                    _urls = new ManagerBase<Url>(_context,this);
                return _urls;
            }
        }
        public ManagerBase<Pick> Picks
        {
            get
            {
                if (_picks == null)
                    _picks = new PickManager(_context,this);
                return _picks;
            }
        }

        public ManagementService() { _context = new MiaopassContext(); }
        public ManagementService(MiaopassContext context) { _context = context; }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
