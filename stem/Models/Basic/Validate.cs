using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace stem.Models.Basic
{
    /// <summary>
    /// 检验用户是否被顶下线,User类的过滤器
    /// </summary>
    public class Validate_UserAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string userid;
            if (httpContext.Request["uid"] == null && httpContext.Request.QueryString["uid"] == null)
            {
                return false;
            }
            if (httpContext.Request.RequestType == "POST")
            {
                userid = httpContext.Request["uid"].ToString();
            }
            else
            {
                userid = httpContext.Request.QueryString["uid"].ToString();
            };

            return valid(userid);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/auth/login/relogin");
            //base.HandleUnauthorizedRequest(filterContext);
        }
        private bool valid(string userid)
        {
            try
            {
                string[] yzm_date = Regex.Split(userid, "asd", RegexOptions.IgnoreCase);
                string iscunzai = Redis.getHash("user", yzm_date[0]);
                if (userid == iscunzai)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch
            {
                return false;
            }

        }
    }
}