using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading;
using System.IO;
using mp.DAL;
using EntityFramework.Extensions;


public static class Downloader
{
    static MiaopassContext _db = new MiaopassContext();

    static int _downloadThreadCount = 0;

    static int _maxDownloadThreadCount = 3;

    static Thread _mainThread = null;

    static string _baseDirectory = HttpContext.Current.Server.MapPath("~/");

    public static void Start()
    {
        if (_mainThread == null)
        {
            _mainThread = new Thread(new ThreadStart(MainThreadWorker));
            _mainThread.IsBackground = true;
            _mainThread.Start();
        }
    }

    static void MainThreadWorker()
    {
        //将所有已开始但未完成的所有任务设置为未开始
        _db.Downloads.Where(i => i.State != DownloadStates.Finished).Update(i => new Download() { State = DownloadStates.NotBegin });

        while (true)
        {
            if (_downloadThreadCount >= _maxDownloadThreadCount)
            {
                Thread.Sleep(1000);
                continue;
            }

            var download = _db.Downloads.Where(i => i.State == DownloadStates.NotBegin).FirstOrDefault();
            if (download == null)
            {
                Thread.Sleep(1000);
                continue;
            }

            download.State = DownloadStates.Processing;
            _db.Entry(download).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            Thread t = new Thread(new ParameterizedThreadStart(DownloadThredWorker));
            t.IsBackground = true;
            t.Start(download);
            _downloadThreadCount++;
        }
    }

    static string DownloadFile(string source, string from, string filepath)
    {
        WebClient wc = new WebClient();
        wc.Headers.Add("Referer", from);
        wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; rv:35.0) Gecko/20100101 Firefox/35.0");
        int tryCount = 0;
        while (tryCount < 5)
        {
            try
            {
                wc.DownloadFile(source, filepath);
                break;
            }
            catch { tryCount++; }
        }

        if (tryCount == 5)
            return "";
        else
            return Tools.FileMd5(filepath);
    }

    static void DownloadThredWorker(object obj)
    {
        var download = obj as Download;

        string hash1 = "";
        string hash2 = "";
        string filePath1 = "";
        string filePath2 = "";
        int tryCount = 0;

        filePath1 = _baseDirectory + "mp_done\\" + Guid.NewGuid().ToString();
        hash1 = DownloadFile(download.Source, download.From, filePath1);
        //尝试下载5次
        while (tryCount < 5)
        {
            filePath2 = _baseDirectory + "mp_done\\" + Guid.NewGuid().ToString();
            hash2 = DownloadFile(download.Source, download.From, filePath2);

            if (hash1 != hash2)
            {
                System.IO.File.Delete(filePath1);

                filePath1 = filePath2;
                hash1 = hash2;

                tryCount++;

                continue;
            }
            else
            {
                break;
            }
        }

        try
        {
            if (hash1 == "" || hash2 == "")
                throw new Exception("文件下载失败");

            mp.DAL.File file = null;
            using (var filestream = System.IO.File.OpenRead(filePath1))
            {
                var manager = new mp.BLL.FileManager(_db);
                manager.Create(filestream, hash1);
            }

            //将下载任务标记为已完成
            download.State = DownloadStates.Finished;
            download.FileID = file.ID;
            _db.Entry(download).State = System.Data.Entity.EntityState.Modified;

            //完善对应的pick任务
            var picks = _db.Picks.Where(i => i.DownloadID == download.ID);
            var imageList = picks.Select(pick => new mp.DAL.Image()
            {
                Description = pick.Description,
                FileID = download.FileID,
                Url = pick.From,
                PackageID = pick.PackageID,
                UserID = pick.UserID,
                Via = 0
            });

            //var imageList = new List<db.Models.Image>();
            //foreach (var pick in picks)
            //{
            //    var image = new db.Models.Image()
            //    {
            //        Description = pick.Description,
            //        FileID = download.FileID,
            //        Url = pick.From,
            //        PackageID = pick.PackageID,
            //        UserID = pick.UserID,
            //        Via=0 
            //    };
            //    imageList.Add(image);
            //}
            _db.Images.AddRange(imageList);
            picks.Delete();

            _db.SaveChanges();
        }
        catch { }
        finally
        {
            System.IO.File.Delete(filePath1);
            System.IO.File.Delete(filePath2);
            _downloadThreadCount--;
        }
    }
}