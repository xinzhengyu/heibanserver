using stem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stem.Areas.Auth.Controllers
{
    public class MyController : Controller
    {
        // GET: Auth/My
        stemEntities bd = new stemEntities();
        /// <summary>
        /// 获取我的左侧公共菜单栏
        /// </summary>
        /// <returns></returns>
        public ActionResult tables_common()
        {
            var query = bd.tables_common.Where(p => p.pre_id == 0&&p.is_use==true).OrderBy(p => p.shunxu)
                .Select(p => new
                {
                    p.name,
                    p.id,
                    p.icon,
                    children=bd.tables_common.Where(a=>a.pre_id==p.id&&p.is_use==true).OrderBy(a=>a.shunxu)
                    .Select(a => new
                    {
                        a.name,
                        a.id,
                        a.is_use,
                        a.icon,
                        a.router
                    })
                });
            return Json(query , JsonRequestBehavior.AllowGet);
        }
    }
}