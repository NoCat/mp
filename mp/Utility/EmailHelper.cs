using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;


    public class EmailHelper
    {
        static string account = "system@miaopass.net";
        static string password = "V8lLt9zrfjkrYqKV";

        public static void Send(string to,string title,string body)
        {
            MailMessage mailObj = new MailMessage();
            mailObj.From = new MailAddress(account); //发送人邮箱地址
            mailObj.IsBodyHtml = true;//正文为html格式
            mailObj.To.Add(to);   //收件人邮箱地址
            //mailObj.CC.Add(""); //抄送
            mailObj.Subject = title;    //主题
            mailObj.Body = body;    //正文
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.qq.com";         //smtp服务器名称
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(account, password);  //发送人的登录名和密码
            smtp.Send(mailObj);
        }
    }