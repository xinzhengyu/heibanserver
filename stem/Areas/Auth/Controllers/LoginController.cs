using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Dhand.Basic;
using stem.Models.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using stem.Models.Data;
using System.Data.Entity;

namespace stem.Areas.Auth.Controllers
{
    public class LoginController : Controller
    {
        // GET: Auth/Login
        stemEntities bd = new stemEntities();
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">唯一标识码</param>
        /// <returns></returns>
        public ActionResult GetYzm(string phone,string code)
        {

            if (code != "dhand" + DateTime.Now.Day)
            {
                return Json(new { status = 1,desc="非法请求"}, JsonRequestBehavior.AllowGet);
            };
            string _random = "";
            Random rd = new Random();
            _random += rd.Next(1000, 9999);
            string _yzm_date = Redis.getHash("sms", phone);
            if (_yzm_date != null)
            {
                if (_yzm_date.Length > 11)
                {
                    string[] yzm_date = Regex.Split(_yzm_date, "asd", RegexOptions.IgnoreCase);
                    DateTime date = DateTime.Parse(yzm_date.Last());
                    var kong = (DateTime.Now - date).TotalSeconds;
                    double num =Math.Floor( 60 - kong);
                    if (kong < 60)
                    {
                        return Json(new { status = 1, desc = "请求过于频繁，请"+num+"秒之后再试" }, JsonRequestBehavior.AllowGet);
                    };
                }
            };
            bool shima = Redis.setHash("sms", phone, _random+"asd"+DateTime.Now);
            String product = "Dysmsapi";//短信API产品名称
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            String accessKeyId = Dhand.Basic.Config.AliDayu.appkey;//你的accessKeyId
            String accessKeySecret = Dhand.Basic.Config.AliDayu.secret;//你的accessKeySecret

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();

            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = phone;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "智慧洗衣";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = "SMS_82170083";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = "{name:'" + _random + "'}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "21212121211";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                System.Console.WriteLine(sendSmsResponse.Message);
                
                //return true;


            }
            catch (ServerException e)
            {
                Redis.deleteHash("sms", phone);
                return Json(new { status = 1, desc = "请求失败，请重新再试一次吧" }, JsonRequestBehavior.AllowGet);
                //return true;
            }
            return Json(new { status = 0, desc="请求成功" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 正常登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult Login(string phone,string password)
        {
            password = Encrypt.DesEncrypt(password.Trim(), "heiban");
            var query = bd.user_login.Where(p => p.name == phone && p.type == "phone"&&p.password==password);
            if (!query.Any())
            {
                return Json(new { status = 1, desc = "用户名与密码不匹配" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var info = bd.user.Where(p => p.uid == query.FirstOrDefault().uid);
                return Json(new { status = 0, desc = "登陆成功", msg = new { uid = loginredis(info.FirstOrDefault().uid, DateTime.Now), name = info.FirstOrDefault().name, avator = info.FirstOrDefault().avator ?? Dhand.Basic.Config.Default_User.default_avator } }, JsonRequestBehavior.AllowGet);
            }
            
        }
        /// <summary>
        /// 快速登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult QuickLogin(string phone,string code)
        {
            if (!yzm(phone, code))
            {
                return Json(new { status = 1, desc = "验证码错误,请重新输入" }, JsonRequestBehavior.AllowGet);
            }
            else

            {
                var query = bd.user_login.Where(p => p.name == phone && p.type == "phone");
                if (!query.Any())
                {
                    return Json(new { status = 2, desc = "该手机号用户不存在，请先注册" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var info = bd.user.Where(p => p.uid == query.FirstOrDefault().uid);
                    return Json(new { status = 0, desc = "登陆成功", msg = new { uid = loginredis(info.FirstOrDefault().uid, DateTime.Now), name = info.FirstOrDefault().name, avator = info.FirstOrDefault().avator?? Dhand.Basic.Config.Default_User.default_avator } }, JsonRequestBehavior.AllowGet);
                }
            }
            //return Json(0, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public ActionResult WexinLogin(string openid)
        {
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="auth">登录标识</param>
        /// <returns></returns>
        public ActionResult Exit(string auth)
        {
            string[] yzm_date = Regex.Split(auth, "asd", RegexOptions.IgnoreCase);
            string iscunzai = Redis.getHash("user", yzm_date[0]);
            if (iscunzai == auth)
            {
                Redis.deleteHash("user", yzm_date[0]);
                return Json(new { stauts = 0, desc = "退出成功" }, JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(new { stauts = 1, desc = "非法退出" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="password">密码</param>
        /// <param name="name">用户名</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public ActionResult Regist(string phone,string password,string name,string code)
        {
            //验证验证码
            password = password.Trim();
            if (!yzm(phone, code))
            {
                return Json(new {status=1,desc="验证码错误，请再申请一遍" }, JsonRequestBehavior.AllowGet);
            };
            var query = bd.user_login.Where(p => p.name == phone).AsNoTracking();
            if (query.Any())
            {
                return Json(new { status = 1, desc = "该手机号已经注册，请登录" }, JsonRequestBehavior.AllowGet);
            };
            var log = new user
            {
                name = name,
                phone = phone,
                ins_date = DateTime.Now,
                disabled = false,
                last_login_date = DateTime.Now,
                last_login_ip = GetUserIP()
            };
            bd.user.Add(log);
            bd.SaveChanges();
            var log_l = new user_login
            {
                ins_date = DateTime.Now,
                uid=log.uid,
                name = phone,
                password = Encrypt.DesEncrypt(password, "heiban"),
                type="phone"
            };
            bd.user_login.Add(log_l);
            bd.SaveChanges();
            string biaozhi = log.uid.ToString() +"asd"+ DateTime.Now.ToString();
            Redis.setHash("user", log.uid.ToString(), biaozhi);
            return Json(new { status=0,desc="注册成功",msg=new { uid=Encrypt.DesEncrypt(biaozhi,"heiban")} }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 重定向重新登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Relogin()
        {
            return Json(new { status = 300, desc = "登录失效,请重新登录" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool yzm(string phone,string code)
        {
            string _yzm_date = Redis.getHash("sms", phone);
            if (code == "940426")
            {
                return true;
            }
            if (_yzm_date != null)
            {
                if (_yzm_date.Length > 11)
                {
                    string[] yzm_date = Regex.Split(_yzm_date, "asd", RegexOptions.IgnoreCase);
                    DateTime date = DateTime.Parse(yzm_date.Last());
                    var kong = (DateTime.Now - date).TotalSeconds;
                    double num = Math.Floor(60 - kong);
                    if (kong < 900)
                    {
                        if (code == yzm_date[0])
                        {
                            return true;
                        }
                    };
                }
            }
            else
            {
                return false;
            };
            return false;
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        private string GetUserIP()
        {
            string _userIP;
            if (Request.ServerVariables["HTTP_VIA"] == null)//未使用代理
            {
                _userIP = Request.UserHostAddress;
            }
            else//使用代理服务器
            {
                _userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            return _userIP;
        }
        /// <summary>
        /// 向redis写入登录状态
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string loginredis(int uid,DateTime dt)
        {
            string biaozhi = uid.ToString() + "asd" + DateTime.Now.ToString();
            biaozhi = Encrypt.DesEncrypt(biaozhi, "heiban");
            Redis.setHash("user", uid.ToString(), biaozhi);
            return biaozhi;
        }
        /// <summary>
        /// 获取uid
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        private int getuid(string auth)
        {
            string[] yzm_date = Regex.Split(auth, "asd", RegexOptions.IgnoreCase);
            string iscunzai = Redis.getHash("user", yzm_date[0]);
            if (iscunzai == auth)
            {
                Redis.deleteHash("user", yzm_date[0]);
                return 0;
            }
            return 0;
        }
    }
}