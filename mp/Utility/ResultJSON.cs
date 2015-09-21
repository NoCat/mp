using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ResultJSON
{
    public int Code { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }

    public ResultJSON()
    {
        Code = 0;
        Message = "OK";
        Data = null;
    }
}