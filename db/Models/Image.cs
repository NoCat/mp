using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.Models
{
    public class Image
    {
        public int ID { get; set; }
        public int PackageID { get; set; }
        virtual public Package Package { get; set; }
        public int UserID { get; set; }
        virtual public User User { get; set; }
        public int FileID { get; set; }
        virtual public File File { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Url { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public int Via { get; set; }

        
        public string ImageHost = System.Configuration.ConfigurationManager.AppSettings["ImageHost"];
        [NotMapped]
        public string OriginUrl
        {
            get
            {
                return ImageHost + File.MD5 + ".jpg";
            }
        }
        [NotMapped]
        public string Fw236Url
        {
            get
            {
                return ImageHost + File.MD5 + "_fw236.jpg";
            }
        }
        [NotMapped]
        public string Fw658Url
        {
            get
            {
                return ImageHost + File.MD5 + "_fw658.jpg";
            }
        }
        [NotMapped]
        public string Fw78Url
        {
            get
            {
                return ImageHost + File.MD5 + "_fw78.jpg";
            }
        }
        [NotMapped]
        public string Sq236Url
        {
            get
            {
                return ImageHost + File.MD5 + "_sq236.jpg";
            }
        }
        [NotMapped]
        public string Sq75Url
        {
            get
            {
                return ImageHost + File.MD5 + "_sq75.jpg";
            }
        }
        public Image()
        {
            CreatedTime = DateTime.Now;
        }


    }
}
