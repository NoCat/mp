using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;


static public class HtmlHelperExtend
{
    static public MvcHtmlString MpDescription(this HtmlHelper helper, string description)
    {
        var regex = new Regex(@"#([^\s#]+?)#");
        if (description == null)
            description = "";
        description = regex.Replace(description, (match) =>
        {
            var str = string.Format("<a class=\"mp-search-link\" href=\"/search?kw={0}\">{1}</a>", HttpUtility.UrlEncode(match.Groups[1].Value), match.Value);
            return str;
        });
        return new MvcHtmlString(description);
    }
}