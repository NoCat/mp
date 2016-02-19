using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Topshelf;

namespace mp.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<Service>();
                x.RunAsLocalSystem();                            //6                

                x.SetDescription("mp后台服务");        //7
                x.SetDisplayName("mp后台服务");                       //8
                x.SetServiceName("mp后台服务");                       //9
            });                                                  //10
        }
    }
}
