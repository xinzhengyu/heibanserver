using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Web.Mvc;
using stem.Models.Basic;
using stem.Models.Data;

namespace stem.Areas.Product.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product/Product
        stemEntities bd = new stemEntities();
        /// <summary>
        /// 收藏接口
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="productid"></param>
        /// <returns></returns>
        [Validate_User]     
        public ActionResult Collect(string uid,int productid)
        {
            int userid = Redis.getUserId(uid);
            var query = bd.product_collect.Where(p => p.product_id == productid && p.uid == userid);
            if (query.Any())
            {
                if (query.FirstOrDefault().is_zan == true)
                {
                    query.FirstOrDefault().is_zan = false;
                    query.FirstOrDefault().qu_date = DateTime.Now;
                    bd.SaveChanges();
                    return Json(new { status = 1, desc = "取消收藏成功" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    query.FirstOrDefault().is_zan = true;
                    query.FirstOrDefault().ins_date = DateTime.Now;
                    return Json(new { status = 0, desc = "收藏成功" }, JsonRequestBehavior.AllowGet);

                };

            }
            else
            {
                var add = new product_collect
                {
                    ins_date = DateTime.Now,
                    is_zan = true,
                    uid = userid,
                    product_id = productid
                };
                bd.product_collect.Add(add);
                bd.SaveChanges();
                return Json(new { status = 0, desc = "收藏成功" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 列表接口
        /// </summary>
        /// <returns></returns>
        public ActionResult Lists()
        {

            return Json(0, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 详情接口
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            return Json(0, JsonRequestBehavior.AllowGet);
        }

    }
}