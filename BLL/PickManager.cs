using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mp.DAL;

namespace mp.BLL
{
    public class PickManager : ManagerBase<Pick>
    {
        public PickManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }

        public Pick Add(string from, string source, int packageId, string description)
        {
            var pick = new Pick();

            var sourceCRC32 = (int)source.CRC32();
            var fromCRC32 = (int)from.CRC32();

            var sourceUrl = Collection.Urls.CreateIfNotExist(new Url { Text = source, CRC32 = sourceCRC32 }, (u => u.CRC32 == sourceCRC32 && u.Text == source));
            var fromUrl = Collection.Urls.CreateIfNotExist(new Url { Text = from, CRC32 = fromCRC32 }, (u => u.CRC32 == fromCRC32 && u.Text == from));
            var package = Collection.Packages.Find(packageId);

            //根据sourceUrl查找下载列表
            var download = Collection.Downloads.CreateIfNotExist(new Download { FromUrlID = fromUrl.ID, SourceUrlID = sourceUrl.ID }, d => d.SourceUrlID == sourceUrl.ID);
            //创建Image
            var image = new Image
            {
                Description = description,
                FileID = download.FileID,
                PackageID = package.ID,
                UserID = package.UserID,
                FromUrlID = fromUrl.ID
            };
            Collection.Images.Add(image);
            //判断文件是否已经下载过
            if (download.FileID != 0)
            {
                //文件已经下载过，直接添加到图包
                pick.ImageID = image.ID;
            }
            else
            {
                //文件未下载过，检查pick是否存在,不存在则添加      
                pick.DownloadID = download.ID;
                pick.FromUrlID = fromUrl.ID;
                pick.PackageID = package.ID;
                pick.UserID = package.UserID;
                pick.Description = description;
                pick.ImageID = image.ID;

                Collection.Picks.Add(pick);
            }
            return pick;
        }
    }
}
