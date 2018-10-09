using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using stem.Models.Data;
using stem.Models.Basic;

namespace stem.Areas.Lessons.Controllers
{
    public class LessonsController : Controller
    {
        stemEntities bd = new stemEntities();
        // GET: Lessons/Lessons
        public ActionResult myLesson(string uid,int page = 0)
        {
            int userid = Redis.getUserId(uid);
            var query = (from p in bd.lesson_user
                        from q in bd.lesson
                        from z in bd.user
                        where p.lessonid == q.id && p.uid == z.uid &&p.uid ==userid
                        orderby p.id descending
                        select new
                        {
                            q.id,
                            q.name,
                            q.jigou,
                            q.price,
                            p.records,
                            q.image,
                           
                        }).Skip(page*5).Take(5);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 系列课程列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Lists(string search,int shopid=0,int page=0,string uid=null)
        {
            search s = JsonConvert.DeserializeObject<search>(search);
            var query = bd.lesson.Select(p => p);
            int userid = 0;
            try
            {
                 userid = Redis.getUserId(uid);
            }
            catch { };
            if (s.type != null)
            {
                if(s.type!=9)
                query = query.Where(p => p.language_id == s.type).Select(p => p);
            };
            if (s.age_min != null)
            {
                query = query.Where(p => p.age_min >= s.age_min).Select(p => p);
            };
            if (s.age_max != null)
            {
                query = query.Where(p => p.age_max <= s.age_max).Select(p => p);
            };
            if (s.kw != null)
            {
                query = query.Where(p => p.name.Contains(s.kw)||p.tag.Contains(s.kw));
            };
            if (shopid != 0)
            {
                query = query.Where(p => p.shop_id == shopid);
            }
            if (s.is_free != null)
            {
                if (s.is_free == true)
                {
                    query = query.Where(p => p.price == 0);
                }else
                {
                    query = query.Where(p => p.price > 0);
                }
            }
             
            return Json(new { status = 0, msg = query.OrderByDescending(p=>p.last_change_date)
                .Skip(page*10).Take(10).
                Select(p => new {
                    p.last_change_date,
                    p.price, p.desc,
                    p.id, p.image,
                    p.jigou,p.shop_id,p.teachers, p.name, p.uid,
                    p.num,p.month_num,
                    language = bd.language_type.Where(a => a.id == p.language_id).Select(a => a.name).FirstOrDefault()
                    ,is_collect=bd.lessson_collect.Where(a=>a.is_zan==true&&a.lessonid==p.id&&a.uid==userid).Count()
                }),total=query.Count() },JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult L_Lists(string search,bool? is_free=null,int page = 0)
        {
            search s = JsonConvert.DeserializeObject<search>(search);
            var query = bd.product.Where(p=>p.type==1&&p.status==1).Select(p => p);
            if (is_free != null)
            {
                query = query.Where(p => p.is_free == is_free).Select(p => p);
            }
            if (s.type != null)
            {
                if (s.type != 9) {
                query = query.Where(p => p.language_type == s.type).Select(p => p);
                }
            };
            if (s.age_min != null)
            {
                query = query.Where(p => p.old_mini >= s.age_min).Select(p => p);
            };
            if (s.age_max != null)
            {
                query = query.Where(p => p.old_max <= s.age_max).Select(p => p);
            };
            if (s.kw != null)
            {
                query = query.Where(p => p.title.Contains(s.kw)||p.sub_title.Contains(s.kw));
            };
            var msg = query.OrderByDescending(p => p.ins_date).Skip(page * 10).Take(10).Select(p => new
            {
                p.id,
                p.image,
                p.video,
                p.pdf,
                p.codezip,
                p.title,
                p.sub_title,
                p.is_free,
                p.price,
                p.sale_num,
                lesson=bd.lesson.Where(a=>a.id==p.lessonid).FirstOrDefault(),
                p.ins_date,
                language=bd.language_type.Where(a=>a.id==p.language_type).FirstOrDefault().name
               
            });
            return Json(new { status = 0, msg =msg }, JsonRequestBehavior.AllowGet);
        }
       //添加学习   
        public ActionResult addStudy(int lessonid,string uid)
        {
            int userid = 0;
            try
            {
                userid = Redis.getUserId(uid);
            }
            catch
            {
                return Json(new { status = 1, desc = "登录过期请先登录" }, JsonRequestBehavior.AllowGet);
            };
            var query = bd.lesson_user.Where(p => p.lessonid == lessonid && p.uid == userid);
            if (query.Any())
            {
                if (query.FirstOrDefault().is_study == true)
                {
                    return Json(new { status = 1, desc = "已经加入过了，不要重复加入" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    query.FirstOrDefault().is_study = true;
                    bd.SaveChanges();
                    return Json(new { status = 0, desc = "加入成功" }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                var newlu = new lesson_user
                {
                    ins_date = DateTime.Now,
                    is_study = true,
                    lessonid = lessonid,
                    uid = userid,
                    records = "[]"
                };
                bd.lesson_user.Add(newlu);
                bd.SaveChanges();
                return Json(new { status = 0, desc = "加入成功" }, JsonRequestBehavior.AllowGet);

            };
            
        }
        /// <summary>
        /// 查找某一套系列课程
        /// </summary>
        /// <param name="lessonid"></param>
        /// <returns></returns>
        public ActionResult Lesson(int lessonid,string uid)
        {
            int userid = 0;
            try
            {
                userid = Redis.getUserId(uid);
            }
            catch { };
            var detail = bd.lesson.Where(p => p.id == lessonid).Select(p=>new {
                p.uid,
                p.teachers,
                p.tag,
                p.shop_id,
                p.price,
                p.num,
                p.name,
                p.month_num,
                p.last_change_date,
                p.language_id,
                p.jigou,
                p.is_xilie,
                p.ins_date,
                p.image,
                p.id,
                p.desc,
                p.content,
                p.age_min,
                p.age_max,
                is_study=bd.lesson_user.Where(a=>a.is_study==true&&a.uid==userid&&a.id==lessonid).Count(),
                is_collect = bd.lessson_collect.Where(a => a.is_zan == true && a.lessonid == p.id && a.uid == userid).Count()
            }).ToList();
            var query = bd.lessons_menu.Where(p=>p.lesson_id==lessonid&&p.pre_id==0).Select(p => new
            {
                p.id,
                p.title,
                p.orderby,
                p.ins_date,
                children=bd.lessons_menu.Where(a=>a.pre_id==p.id).Select(a=>new {
                    a.id,
                    a.title,
                    a.orderby,
                    a.ins_date,
                    a.product_id,
                    a.is_free,
                    a.price,
                    a.time
                })

            });
            return Json(new { menu = query,status=0,detail=detail.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 单一课程详情
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public ActionResult Detail(int productid, string uid = null)
        {
            int userid = 0;
            try
            {
                userid = Redis.getUserId(uid);
            }
            catch { };
            var query = bd.product.Where(p => p.id == productid).Select(p => new
            {
                p.id,
                p.image,
                video=bd.lesson_video.Where(a=>a.id==p.video).Select(a=>new {
                   a.url,
                   a.size
                }),
                p.pdf,
                p.codezip,
                p.title,
                p.sub_title,
                p.is_free,
                p.price,
                p.sale_num,
                lesson = bd.lesson.Where(a => a.id == p.lessonid).FirstOrDefault(),
                p.ins_date,
                language = bd.language_type.Where(a => a.id == p.language_type).FirstOrDefault().name,
                p.imglist,
           });
            if (query.Any())
            {
                return Json(new { status = 0, msg = query.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

            };
            return Json(new { status = 1}, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 课程评论详情
        /// </summary>
        /// <param name="lessonid"></param>
        /// <returns></returns>
        public ActionResult Lesson_Comment(int lessonid,int page=0,int num=10,string type="all")
        {
            var rate = bd.lesson_comment.Where(p => p.lesson_id == lessonid && p.pre_id == 0).Select(p => p.first_star);
            var query = bd.lesson_comment.Where(p => p.lesson_id == lessonid && p.pre_id == 0).Select(p => new
            {
                p.desc,
                p.imglist,
                p.first_star,
                p.second_star,
                p.third_star,
                p.ins_date,
                user = bd.user.Where(a => a.uid == p.uid).Select(a => new { a.name, a.avator }).FirstOrDefault(),
                children = bd.lesson_comment.Where(a => a.lesson_id == lessonid && a.pre_id == p.id).Select(a => new
                {
                    a.desc,
                    a.ins_date,
                    a.toname,
                    a.uname

                })
            }).OrderByDescending(p => p.ins_date).Select(p=>p);
            if (type == "hao")
            {
                query = query.Where(p => p.first_star >= 4);
            };
            if (type == "zhong")
            {
                query = query.Where(p => p.first_star <4&&p.first_star>=3);
            };
            if (type == "cha")
            {
                query = query.Where(p => p.first_star < 3);
            };
            if (type == "tu")
            {
                query = query.Where(p => p.imglist != "[]");
            }
            query=query.Skip(page * num).Take(num);
            return Json(new { msg = query, status = 0, rate = new { total = rate.Average(p => p)??5,
                hao = rate.Where(p => p >= 4).Count(),
                zhong =rate.Where(p=>p>=3&p<4).Count(),
                cha =rate.Where(p=>p<3).Count(),
                tu =bd.lesson_comment.Where(p=>p.imglist!="[]"&&p.lesson_id==lessonid).Count()
            } }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 收藏/取消收藏课程
        /// </summary>
        /// <param name="lessonid"></param>
        /// <returns></returns>
        public ActionResult Lesson_Collect(int lessonid,string uid)
        {
            int userid = Redis.getUserId(uid);
            var query = bd.lessson_collect.Where(p => p.lessonid == lessonid && p.uid == userid);
            if (query.Any())
            {
                if (query.FirstOrDefault().is_zan == true)
                {
                    query.FirstOrDefault().is_zan = false;
                    bd.SaveChanges();
                    return Json(new { status = 1, desc = "取消收藏" }, JsonRequestBehavior.AllowGet);
                }else
                {
                    query.FirstOrDefault().is_zan = true;
                    bd.SaveChanges();
                    return Json(new { status = 0, desc = "收藏成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var add = new lessson_collect
                {
                    ins_date = DateTime.Now,
                    lessonid = lessonid,
                    uid = userid,
                    is_zan = true
                };
                bd.lessson_collect.Add(add);
               
            };
            bd.SaveChanges();
            return Json(new { status = 0, desc = "收藏成功" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 关键字
        /// </summary>
        /// <returns></returns>
        public ActionResult keywords()
        {
            List<keyword> lists = new List<keyword>() { };
            keyword kw1 = new keyword
            {
                id = 0,
                type = "kw",
                title = "Scratch",
                biaozhi="lesson"
            };
            keyword kw3 = new keyword
            {
                id = 0,
                type = "kw",
                title = "人工智能",
                biaozhi = "lesson"
            };
            lists.Add(kw3);
            keyword kw4 = new keyword
            {
                id = 0,
                type = "kw",
                title = "信息学奥赛",
                biaozhi = "lesson"
            };
            lists.Add(kw4);
            keyword kw2 = new keyword
            {
                id = 1,
                type = "detail",
                title = "实战京东物流车",
                biaozhi = "lesson"
            };
            lists.Add(kw2);
            return Json(lists,JsonRequestBehavior.AllowGet);
        }
        public class keyword
        {
            public string title { get; set; }
            public string type { get; set; }
            public int? id { get; set; }
            public string biaozhi { get; set; }
        }
        public class search
        {
            public int? type { get; set; }
            public int? age_min { get; set; }
            public int? age_max { get; set; }
            public string kw { get; set; }
            public bool? is_free { get; set; }
        }
        
    }
}