using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;
using mp.BLL;
using System.Threading;
using System.Net;

namespace mp.Utility
{
    public class DownloadJob : Job
    {
        int maxThreadsCount = 1;
        int currentThreadsCount = 0;
        ManagerCollection manager = new ManagerCollection();
        static string baseDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/");

        protected override void ExcuteCore(object param)
        {
            //将所有未完成的任务设置为未开始
            manager.Downloads.Items.Where(d => d.State == DownloadStates.Processing).ToList().ForEach(d =>
            {
                d.State = DownloadStates.NotBegin;
                manager.Downloads.Update(d);
            });

            while (true)
            {
                if (currentThreadsCount == maxThreadsCount)
                {
                    Thread.Sleep(200);
                    continue;
                }

                var download = manager.Downloads.Items.Where(d => d.State == DownloadStates.NotBegin).OrderByDescending(d=>d.ID).FirstOrDefault();
                if (download == null)
                {
                    Thread.Sleep(200);
                    continue;
                }

                currentThreadsCount++;
                download.State = DownloadStates.Processing;
                manager.Downloads.Update(download);
                var t = new Thread(new ParameterizedThreadStart(Download));
                t.IsBackground = true;
                t.Start(download);
            }
        }

        void Download(object obj)
        {
            var download = obj as Download;

            string hash1 = "";
            string hash2 = "";
            string filePath1 = baseDirectory + "temp\\" + Guid.NewGuid().ToString();
            string filePath2 = baseDirectory + "temp\\" + Guid.NewGuid().ToString();
            int tryCount = 0;

            hash1 = DownloadFile(download.SourceUrl.Text, download.FromUrl.Text, filePath1);
            //尝试下载5次
            while (tryCount < 5)
            {
                hash2 = DownloadFile(download.SourceUrl.Text, download.FromUrl.Text, filePath2);

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

                manager.Transaction(() =>
                {
                    mp.DAL.File file = null;
                    using (var filestream = System.IO.File.OpenRead(filePath1))
                    {
                        file = manager.Files.Add(filestream);
                    }

                    //将下载任务标记为已完成
                    download.State = DownloadStates.Finished;
                    download.FileID = file.ID;
                    manager.Files.Update(file);

                    //完善对应的pick任务
                    var picks = manager.Picks.Items.Where(i => i.DownloadID == download.ID);
                    var imageList = new List<DAL.Image>();
                    picks.ToList().ForEach(pick => imageList.Add(
                        new mp.DAL.Image()
                        {
                            Description = pick.Description,
                            FileID = download.FileID,
                            FromUrlID = pick.FromUrlID,
                            PackageID = pick.PackageID,
                            UserID = pick.UserID,
                            Via = 0
                        }));


                    imageList.ToList().ForEach(i => { manager.Images.Add(i); });
                    picks.ToList().ForEach(p => { manager.Picks.Remove(p); });
                });
            }
            catch { }
            finally
            {
                System.IO.File.Delete(filePath1);
                System.IO.File.Delete(filePath2);
            }


            currentThreadsCount--;
        }

        string DownloadFile(string source, string from, string filepath)
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
                return filepath.FileMD5();
        }

    }
}