using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using stem.Models.Data;
using Dhand.Basic;
using stem.Models.Basic;
using Newtonsoft.Json;


namespace stem.Areas.Product.Controllers
{
    public class ShopsController : Controller
    {
        // GET: Product/Shops
        stemEntities bd = new stemEntities();
        /// <summary>
        /// 首页推荐
        /// </summary>
        /// <returns></returns>
        public ActionResult shouyetuijian()
        {
            var query = bd.shop.OrderBy(p => p.id).Take(5).Select(p=>p);

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 商城详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShopDetail(int id)
        {
            var lessons = (from p in bd.lesson
                          from q in bd.lesson_comment
                          where p.shop_id == id && p.id == q.lesson_id
                          select p.id).ToList();
            var average = bd.lesson_comment.Where(p => lessons.Contains(p.lesson_id)).Average(p => p.first_star)??5;
            average = Math.Round(average, 2);
            string taverage = average.ToString("0.00");
            var query = bd.shop.Where(p => p.id == id).Select(p => new
            {
                p.id,
                p.ins_date,
                p.jianjie,
                p.logo,
                p.name,
                p.type,
                p.banner,
                kecheng_num=bd.lesson.Where(a=>a.shop_id==id).Count(),
                xuexi_num=bd.lesson.Where(a=>a.shop_id==id).Sum(a=>a.num),
                rate=taverage
               
            });
            return Json(query.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 商城主轮播图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShopBanner(int id)
        {
            var query = bd.shops_banners.Where(p => p.shop_id == id).OrderBy(p => p.orderby).Select(p => p);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 商城列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Lists(string kw=null,string city=null,string uid=null,int? type=null,int page=0)
        {
            int userid = 0;
            try
            {
                userid = Redis.getUserId(uid);
            }
            catch { };
            var query = bd.shop.Select(p => p);
            if (type != null)
            {
                query = query.Where(p => p.type == type);
            };
            if (kw != null&&kw!="null"&&kw!="")
            {
                query = query.Where(p => p.name.Contains(kw) || p.jianjie.Contains(kw));
            };
            if (city != null&&city!="null"&&city!="")
            {
                //List<int> citys=
            }
            return Json(new { msg = query.OrderBy(p => p.id).Skip(page * 10).Take(10)
            .Select(p => new {
                p.id,
                p.name,
                p.logo,
                p.jianjie,
                p.ins_date,
                p.type,
                p.status,
                lessons = bd.lesson.OrderByDescending(a=>a.id).Where(a => a.shop_id == p.id).Select(a=>new{
                    a.last_change_date,
                    a.price,
                    a.desc,
                    a.id,
                    a.image,
                    a.jigou,
                    a.shop_id,
                    a.teachers,
                    a.name,
                    a.uid,
                    a.num,
                    a.month_num,
                    language = bd.language_type.Where(z => z.id == a.language_id).Select(z => z.name).FirstOrDefault()
                    ,
                    is_collect = bd.lessson_collect.Where(z => z.is_zan == true && z.lessonid == a.id && a.uid == userid).Count()
                })
            }),total=query.Count() }, JsonRequestBehavior.AllowGet);
        }
    }
}