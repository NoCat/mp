using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Nito.KitchenSink.CRC;

namespace mp.DAL
{
    public class Download
    {
        public int ID { get; set; }
        public int FromUrlID { get; set; }
        virtual public Url FromUrl { get; set; }
        public int SourceUrlID { get; set; }
        virtual public Url SourceUrl { get; set; }
        public DownloadStates State { get; set; }
        public int FileID { get; set; }
        virtual public File File { get; set; }

        public Download()
        {
            State = DownloadStates.NotBegin;
        }
    }

    public enum DownloadStates : byte
    {
        /// <summary>
        /// 未开始
        /// </summary>
        NotBegin = 0,
        /// <summary>
        /// 处理中
        /// </summary>
        Processing = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        Finished = 2
    }
}
