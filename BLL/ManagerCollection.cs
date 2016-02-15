using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

namespace mp.BLL
{
    public class ManagerCollection
    {
        MiaopassContext db;

        ManagerBase<User> users;
        public ManagerBase<User> Users
        {
            get
            {
                if (users == null)
                    users = new ManagerBase<User>(db, this);
                return users;
            }
        }

        FileManager files;
        public FileManager Files
        {
            get
            {
                if (files == null)
                    files = new FileManager(db, this);
                return files;
            }
        }

        ImageManager images;
        public ImageManager Images
        {
            get
            {
                if (images == null)
                    images = new ImageManager(db, this);
                return images;
            }
        }


        ManagerBase<Package> packages;
        public ManagerBase<Package> Packages
        {
            get
            {
                if (packages == null)
                    packages = new ManagerBase<Package>(db, this);
                return packages;
            }
        }

        ManagerBase<Download> downloads;
        public ManagerBase<Download> Downloads
        {
            get
            {
                if (downloads == null)
                    downloads = new ManagerBase<Download>(db, this);
                return downloads;
            }
        }

        PickManager picks;
        public PickManager Picks
        {
            get
            {
                if (picks == null)
                    picks = new PickManager(db, this);
                return picks;
            }
        }

        PraiseManager praises;
        public PraiseManager Praises
        {
            get
            {
                if (praises == null)
                    praises = new PraiseManager(db, this);
                return praises;
            }
        }

        ManagerBase<Following> followings;
        public ManagerBase<Following> Followings
        {
            get
            {
                if (followings == null)
                    followings = new ManagerBase<Following>(db, this);
                return followings;
            }
        }

        ManagerBase<Url> urls;
        public ManagerBase<Url> Urls
        {
            get
            {
                if (urls == null)
                    urls = new ManagerBase<Url>(db, this);
                return urls;
            }
        }

        ManagerBase<Config> configs;
        public ManagerBase<Config> Configs
        {
            get
            {
                if (configs == null)
                    configs = new ManagerBase<Config>(db, this);
                return configs;
            }
        }

        ManagerBase<AdminUser> adminUsers;
        public ManagerBase<AdminUser> AdminUsers
        {
            get
            {
                if (adminUsers == null)
                    adminUsers = new ManagerBase<AdminUser>(db, this);
                return adminUsers;
            }
        }

        ManagerBase<AdminSubAccount> adminSubAccounts;
        public ManagerBase<AdminSubAccount> AdminSubAccounts
        {
            get
            {
                if (adminSubAccounts == null)
                    adminSubAccounts = new ManagerBase<AdminSubAccount>(db, this);
                return adminSubAccounts;
            }
        }

        ManagerBase<AdminPixivPickUser> adminPixivPickUsers;
        public ManagerBase<AdminPixivPickUser> AdminPixivPickUsers
        {
            get
            {
                if (adminPixivPickUsers == null)
                    adminPixivPickUsers = new ManagerBase<AdminPixivPickUser>(db, this);
                return adminPixivPickUsers;
            }
        }

        AdminPixivTagManager adminPixivTags;
        public AdminPixivTagManager AdminPixivTags
        {
            get
            {
                if (adminPixivTags == null)
                    adminPixivTags = new AdminPixivTagManager(db, this);
                return adminPixivTags;
            }
        }

        ResaveChainManager resaveChains;
        public ResaveChainManager ResaveChains
        {
            get
            {
                if (resaveChains == null)
                    resaveChains = new ResaveChainManager(db, this);
                return resaveChains;
            }
        }

        AdminPixivWorkTagManager adminPixivWorkTags;
        public AdminPixivWorkTagManager AdminPixivWorkTags
        {
            get
            {
                if (adminPixivWorkTags == null)
                    adminPixivWorkTags =new AdminPixivWorkTagManager(db, this);
                return adminPixivWorkTags;
            }
        }

        ManagerBase<AdminPixivWork> adminPixivWorks;
        public ManagerBase<AdminPixivWork> AdminPixivWorks
        {
            get
            {
                if (adminPixivWorks == null)
                    adminPixivWorks = new ManagerBase<AdminPixivWork>(db, this);
                return adminPixivWorks;
            }
        }

        
        public ManagerCollection()
        {
            db = new MiaopassContext();
        }

        public ManagerCollection(MiaopassContext db)
        {
            this.db = db;
        }

        public void Transaction(Action action)
        {
            db.Transaction(action);
        }
    }
}
