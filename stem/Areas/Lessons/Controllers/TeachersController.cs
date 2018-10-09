using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using stem.Models.Data;
using Dhand.Basic;
using stem.Models.Basic;
using Newtonsoft.Json;

namespace stem.Areas.Lessons.Controllers
{
    public class TeachersController : Controller
    {
        // GET: Lessons/Teachers
        stemEntities bd = new stemEntities();
        /// <summary>
        /// 图像
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult Image(string uid)
        {
            int userid = Redis.getUserId(uid);
            string url = "";
            var file = Request.Files;
            for(int i = 0; i < file.Count; i++)
            {
                url = use_File.save_image(file[i], "teachers", userid, true, 100, 200, 200);
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 上传/修改老师信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult Up(string uid,string form)
        {
            var str = "";
            int userid = Redis.getUserId(uid);
            int shop = bd.shop.Where(p => p.fuzeren_uid == userid).FirstOrDefault().id;
            var shuju = JsonConvert.DeserializeObject<shops_teachers>(form);
            if (shuju.id == 0)
            {
                var add = new shops_teachers
                {
                    birth = shuju.birth,
                    desc = shuju.desc,
                    type = shuju.type,
                    image = shuju.image,
                    ins_date = DateTime.Now,
                    name = shuju.name,
                    school = shuju.school,
                    shop_id = shop,
                };
                bd.shops_teachers.Add(add);
                str = "添加成功";
            }else
            {
                var change = bd.shops_teachers.Where(p => p.id == shuju.id);
                if (change.Any())
                {
                    var c = change.FirstOrDefault();
                    c.birth = shuju.birth;
                    c.desc = shuju.desc;
                    c.type = shuju.type;
                    c.image = shuju.image;
                    c.name = shuju.name;
                    c.school = shuju.school;
                };
                str = "修改成功";
            };
            bd.SaveChanges();
            return Json(new { status = 0, desc=str}, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回老师列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult Lists(string uid)
        {
            int userid = Redis.getUserId(uid);
            int shopid = bd.shop.Where(p => p.fuzeren_uid == userid).FirstOrDefault().id;
            var teachers = bd.shops_teachers.Where(p => p.shop_id == shopid).ToList();
            return Json(new { status = 0, msg = teachers }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除老师
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string uid,int id)
        {
            int userid = Redis.getUserId(uid);
            int shopid = bd.shop.Where(p => p.fuzeren_uid == userid).FirstOrDefault().id;
            var query = bd.shops_teachers.Where(p => p.shop_id == shopid && p.id == id);
            bd.shops_teachers.RemoveRange(query);
            bd.SaveChanges();
            return Json(new { status = 0, desc = "删除成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}