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
        if (description == null)
            description = "";
        var regex = new Regex(@"#(.+?)#");

        description = regex.Replace(description, (match) =>
        {
            var str = string.Format("<a class=\"mp-search-link\" href=\"/search?kw={0}\">{1}</a>", HttpUtility.UrlEncode(match.Groups[1].Value), match.Value);
            return str;
        });

        var regex2 = new Regex(@"(http|https):\/\/[A-Za-z0-9_\-_]+(\.[A-Za-z0-9_\-_]+)+([A-Za-z0-9_\-\.,@?^=%&amp;:/~\+#]*[A-Za-z0-9_\-\@?^=%&amp;/~\+#])?");
        description = regex2.Replace(description, (match) =>
        {
            var str = string.Format("<a class=\"label label-default\" href=\"{0}\" target=\"_blank\">网页链接 &gt;</a>", match.Value);
            return str;
        });
        return new MvcHtmlString(description);
    }
}