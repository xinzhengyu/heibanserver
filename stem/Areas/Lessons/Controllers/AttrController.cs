using stem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stem.Areas.Lessons.Controllers
{
    public class AttrController : Controller
    {
        // GET: Lessons/Attr
        stemEntities bd = new stemEntities();
        /// <summary>
        /// 语言列表
        /// </summary>
        /// <returns></returns>
        public ActionResult language_list(int type=0)
        {
            var query = bd.language_type.Where(p=>p.type==type).OrderBy(p => p.shunxu).Select(p => new 
            {
                p.id,
                p.icon,
                p.is_soft,
                p.name,
                num=bd.lesson.Where(a=>a.language_id==p.id).Count(),
                p.shunxu
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回轮播图
        /// </summary>
        /// <returns></returns>
        public ActionResult banners(string kw="zong")
        {
            var query = bd.system_banner.Where(p=>p.kw==kw).OrderBy(p => p.orderby);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult jsont()
        {
            var query = bd.shop_teachers.ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        
    }
}