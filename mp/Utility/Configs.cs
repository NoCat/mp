using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Configs
{
    public readonly static Uri ImageHost = new Uri(System.Configuration.ConfigurationManager.AppSettings["ImageHost"]);
}