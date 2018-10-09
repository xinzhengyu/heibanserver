using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using stem.Models.Data;
using Dhand.Basic;
using stem.Models.Basic;

namespace stem.Areas.Lessons.Controllers
{
    public class VideoController : Controller
    {
        stemEntities bd = new stemEntities();
        // GET: Lessons/Video
       /// <summary>
       /// 上传
       /// </summary>
       /// <param name="uid"></param>
       /// <returns></returns>
        public ActionResult Up(string uid)
        {
            List<lesson_video> videos = new List<lesson_video>() {};
            try
            {
                var files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    int userid = Redis.getUserId(uid);
                    int shop = bd.shop.Where(p => p.fuzeren_uid == userid).FirstOrDefault().id;
                    string url = use_File.save_file(files[i],"video",userid);
                    var video = new lesson_video
                    {
                        ins_date = DateTime.Now,
                        name = files[i].FileName,
                        shop = shop,
                        size = files[i].ContentLength / 1024 / 1024,
                        uid = userid,
                        url = url
                    };
                    videos.Add(video);
                };
                bd.lesson_video.AddRange(videos);
                bd.SaveChanges();
                return Json(new { status = 0, msg = videos });
            }
            catch
            {
                return Json(new { status = 1, msg = videos });
            }

        }
        /// <summary>
        /// 获取video列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult Lists(string uid)
        {
            int userid = Redis.getUserId(uid);
            var query = bd.lesson_video.Where(p => p.uid == userid).OrderByDescending(p => p.ins_date);
            return Json(new { status = 0, msg = query }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string uid,int id)
        {
            int userid = Redis.getUserId(uid);
            var query = bd.lesson_video.Where(p => p.uid == userid && p.id == id);
            if (!query.Any())
            {
                return Json(new { status = 1, desc = "删除错误，该视频不存在" }, JsonRequestBehavior.AllowGet);

            }
            var str=use_File.delete_file(query.FirstOrDefault().url.Replace(Dhand.Basic.Config.Ip,""));
            bd.lesson_video.Remove(query.FirstOrDefault());
            bd.SaveChanges();
            return Json(new { status = 0, desc = "删除成功" }, JsonRequestBehavior.AllowGet);
            
        }
        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Rename(string uid,int id,string name)
        {
            int userid = Redis.getUserId(uid);
            var query = bd.lesson_video.Where(p => p.uid == userid && p.id == id).ToList();
            if (!query.Any())
            {
                return Json(new { status = 1, desc = "修改错误，该视频不存在" }, JsonRequestBehavior.AllowGet);

            };
            query.FirstOrDefault().name = name;
            bd.SaveChanges();
            return Json(new { status = 0, desc = "修改成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}