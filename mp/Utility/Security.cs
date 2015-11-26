using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using mp.DAL;


public class Security
{
    const string SessionKey = "userid";
    const string CookieKey = "login";

    static public bool IsLogin
    {
        get
        {
            if (HttpContext.Current.Session[SessionKey] != null)
                return true;

            var cookie = HttpContext.Current.Request.Cookies[CookieKey];
            if (cookie == null)
                return false;

            try
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket == null || ticket.Expired == true)
                    return false;

                HttpContext.Current.Session[SessionKey] = Convert.ToInt32(ticket.UserData);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    static public User User
    {
        get
        {
            if (IsLogin == false)
                return null;

            var item = HttpContext.Current.Items["User"];
            if (item != null)
            {
                return item as User;
            }

            var s = new MiaopassContext().Users.Find(HttpContext.Current.Session[SessionKey]);
            HttpContext.Current.Items["User"] = s;
            return s;
        }
    }

    //权限集合功能,暂时不使用
    //static public bool CheckPermissions(params string[] permissions)
    //{
    //    if (IsLogin == false)
    //        return false;

    //    var result = true;
    //    foreach (var p in permissions)
    //    {
    //        var value = false;
    //        try
    //        {
    //            value = Permissions[p];
    //        }
    //        catch { }

    //        result &= value;
    //        if (result == false)
    //            break;
    //    }
    //    return result;
    //}


    //权限集合功能,暂时不使用
    //static Dictionary<string, bool> Permissions
    //{
    //    get
    //    {
    //        if (IsLogin == false)
    //            return null;

    //        var item = HttpContext.Current.Items["Permissions"];
    //        if (item != null)
    //        {
    //            return item as Dictionary<string, bool>;
    //        }

    //        var result = new Dictionary<string, bool>();
    //        var db = new MiaopassContext();
    //        var roles = from ur in db.UserRoles
    //                    where ur.UserID == User.ID
    //                    select new { ur.RoleID };

    //        var permissions = from rp in db.RolePermissions
    //                          join r in roles on rp.RoleID equals r.RoleID
    //                          select new { rp.PermissionID, rp.Value };

    //        var r0 = from p in db.Permissions
    //                 join p1 in permissions on p.ID equals p1.PermissionID
    //                 group p1 by p.Code into g
    //                 select new { g.Key, sum = g.Sum(i => i.Value) };


    //        r0.ToList().ForEach(i =>
    //        {
    //            result.Add(i.Key, i.sum > 0);
    //        });
    //        HttpContext.Current.Items["Permissions"] = result;
    //        return result;
    //    }
    //}

    static public void Login(int userID, bool remember)
    {
        if (remember)
        {
            var now = DateTime.Now;
            var timeout = 15;
            var expire = now.AddDays(timeout);
            var ticket = new FormsAuthenticationTicket(1, userID.ToString(), now, expire, true, userID.ToString());

            var cookie = new HttpCookie(CookieKey);
            cookie.Value = FormsAuthentication.Encrypt(ticket);
            cookie.HttpOnly = true;
            cookie.Expires = expire;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        HttpContext.Current.Session[SessionKey] = userID;
    }

    static public void Logout()
    {
        HttpContext.Current.Response.Cookies.Add(new HttpCookie(CookieKey) { Expires = DateTime.Now.AddDays(-1) });
        HttpContext.Current.Session.Remove(SessionKey);
    }
}