using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mp.DAL;

namespace mp.BLL
{
    public class PickManager:ManagerBase
    {
        public PickManager(MiaopassContext db) : base(db) { }

        public void Add(string from,string source,int packageId,string description)
        {
            var sourceCRC32 = (int)source.CRC32();
            var fromCRC32 = (int)from.CRC32();

            var sourceUrl = DB.UrlCreateIfNotExist(new Url { Text = source, CRC32 = sourceCRC32 }, (u => u.CRC32 == sourceCRC32 && u.Text == source));
            var fromUrl = DB.UrlCreateIfNotExist(new Url { Text = from, CRC32 = fromCRC32 }, (u => u.CRC32 == fromCRC32 && u.Text == from));
            var package = DB.Packages.Find(packageId);

            //根据sourceUrl查找下载列表
            var download = DB.DownloadCreateIfNotExist(new Download { FromUrlID = fromUrl.ID, SourceUrlID = sourceUrl.ID }, d => d.SourceUrlID == sourceUrl.ID);
            //判断文件是否已经下载过
            if (download.FileID != 0)
            {
                //文件已经下载过，直接添加到图包
                DB.ImageInsert(new Image { Description = description, FileID = download.FileID, PackageID = package.ID, UserID = package.UserID, FromUrlID = fromUrl.ID });
            }
            else
            {
                //文件未下载过，添加pick任务
                DB.PickInsert(new Pick { DownloadID = download.ID, FromUrlID = fromUrl.ID, PackageID = package.ID, UserID = package.UserID, Description = description });
            }
        }
    }
}
