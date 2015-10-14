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
        public PickManager(MiaopassContext context,ManagementService service) : base(context,service) { }

        //public void Insert(string source,string from,string description,int packageId,int userId)
        //{
        //    var sourceUrl = Service.Urls.CreateIfNotExist(new Url { Text = source, CRC32 = source.CRC32() }, (u => u.CRC32 == source.CRC32() && u.Text == source));
        //    var fromUrl = Service.Urls.CreateIfNotExist(new Url { Text = from, CRC32 = from.CRC32() }, (u => u.CRC32 == from.CRC32() && u.Text == from));

        //    var download = Service.Downloads.CreateIfNotExist(new Download { FromUrlID = fromUrl.ID, SourceUrlID = sourceUrl.ID }, d => d.SourceUrlID == sourceUrl.ID);
        //    //判断是否下载过了
        //    if (download.FileID != 0)
        //    {
        //        //已经下载过,直接添加图片
        //        var image = new Image
        //        {
        //            Description = description,
        //            FileID = download.FileID,
        //            PackageID = packageId,
        //            UserID = userId,
        //            FromUrlID = fromUrl.ID
        //        };
        //        Service.Images.Insert(image); 
        //    }
        //    else
        //    {
        //        //未下载过的,添加到pick任务
        //        var pick = new Pick
        //        {
        //            DownloadID = download.ID,
        //            Description = description,
        //            FromUrlID = fromUrl.ID,
        //            PackageID = packageId
        //        };
        //        Context.Insert(pick);
        //    }
        //}
    }
}
