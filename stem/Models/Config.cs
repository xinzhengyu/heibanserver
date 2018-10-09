using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhand.Basic
{
    public static class Config
    {
        /// <summary>
        /// 网站ip
        /// </summary>
        public const string Ip = "http://dezhou.dhand.cn/";
        /// <summary>
        /// 阿里大于短信平台账号
        /// </summary>
        public static class AliDayu
        {
            /// <summary>
            /// 阿里大于appkey
            /// </summary>
            public const string appkey = "LTAIv9lXcsCpNXi8";
            /// <summary>
            /// 阿里大于secret
            /// </summary>
            public const string secret = "RbsQSASThkO2prC6wST9b8NuFNhOFy";
        }
        /// <summary>
        /// 默认账户返回的信息
        /// </summary>
        public static class Default_User
        {
            /// <summary>
            /// 默认头像
            /// </summary>
            public const string default_avator = "https://avatar.csdn.net/1/8/D/1_sp1234.jpg";
            /// <summary>
            /// 默认用户名
            /// </summary>
            public const string name = "未命名";

        }
        /// <summary>
        /// 网站基本配置信息
        /// </summary>
        public static class Default_Website
        {
            public const string name = "智慧洗衣后台";
        }
        /// <summary>
        /// redis配置
        /// </summary>
        public static class Redis_Config
        {
            public const string constr = "127.0.0.1:6379";
        }
        /// <summary>
        /// 微信api配置
        /// </summary>
        public static class Wepay_Config
        {

            public const string appid = "wx7c5fb82078936a0d";
            public const string appsecret = "51c56b886b5be869567dd389b3e5d1d6";
            public const string mchid = "1426366602";
            public const string key = "AAAAAAAAAAAAAAAAAzhanglingyu1208";
            public const string notify_url = Ip + "/notify/Wxpay_Notify";
        }
        /// <summary>
        /// 阿里支付配置
        /// </summary>
        public static class Alipay_Config
        {
            public const string appid = "2016080300154512";
            public const string uid = "2088621953722324";
            //public const string uid = "2088011569117780";
            //public const string APP_PRIVATE_KEY ="MIICXAIBAAKBgQDBJYwJldR0d1+fKyMXEW0V5G6+Q1Ec8ULfXEGUQqLo+cfz8xp3n5FSkXHHHtz9i89V4riBymJsp7mbPpmTowRfDKbJTXSuXb4JyuBTWdDF88LsDl0CJzYUz0g2ttTKoPq0QtzGIPrrKaUR+clg5nZSDMdaWI8cgM1rv /xbzpGm4QIDAQABAoGAaktilQfbAzmK60rPJevWL90mQRlE83unBMt9370IcNS + APhCEaFEVb9rcVz /251svmRmjC84GXn4wIIj / McTKNMx7tMS1YPyvln/5sMHf3FA/55x/kZpQbn6peTvL4JYMmImChv0VbYJgD7kE3sw2AKQlaQ3lRC07Gtk+EjRUBECQQDkrGxOSZaLwtIpaG1x/S2or9dH27NTWBis0CykTxvvdU7taWPPZ/zY81rxJ1A67ubjXyBydxma8r7kKGEm4jCFAkEA2DpJbYPFIZi9KISwr53NvDDO3XLCW3YvKOdvSMcqG6TVmU28JlSUBvsiN2J47eC65YwGyckooqa+/6mfAih5rQJAJSnslDCbee1YsIgbOWpXMFIbn78R/tqTcAqJJs8AkEM1Z5o4MW4KBsxOHGEyRdGrzooYQOIgVDFdWsFBQMuD7QJAbSXvDuMnTXBGe/+PGPc1dTvGEK7vQTAFWkaJPFmc/dtVH0/IHCkxl9yJRx/kH2ADt//ZJV7vrjsxyuiapYI8+QJBAM4bLQert91+ASKjJzzEuzlsDD3nyH7CNtRpNr4JZEpf0L5CZy2DXBAjsISoOB5vbMX0tLJiCFRYmKe6iJP8+Ec=";         
            public const string APP_PRIVATE_KEY = "MIIEpAIBAAKCAQEA0yPPEkfd/PC5VXPLlyiLOkx6CNFwE2ef5mFpyCU8+CJt6HvvcsCSi486IR6worKvLoKIFHy5IZPQh+D/QXWoh2k80ZSAzALezW46i/77dUHUlsF+6lMHm5ie05u+xVUyWyDqbiXyaAB8PymmwZ6ct56YVaHeagsv3nXmofArGJtWb8eqUA3Fl7YVHLGbjZlB7uWlx61649iJx2Cef7+m4H7tgoklUwDdrMHvbT3Pgjordz3/LVrXFdHU8CrtyBvTvPlZuwfYnjA0+fBmNFHKV0MeT6+lkh7yzJmI4HcKB/8yp42VioGONIYNwBOllQI0L52IQv7VtuVwU69Yo/rSEQIDAQABAoIBAEE/trViikKprcyvnUIYYHZive/NsYH9qxeKESOuBlp0YVzQOB5RTrhcSc2Ea3fMGoEYC6+xH5E12eKz8I4tKyi1p/rolqj1Vh9MKkGrSdBhyK3SHEOT5yz5jFO+tMTM4GPzFx91EKV9SOhIGeWJ+8pu84q7HD3POTrZWq2+x8KxZk8B48zYJr3zlJBLWj7LbaQ2rVvC2Xn/4PCzWq8tIpBRuKkaRkkhRibgMuqU95UXpvH8ELjmIMEPY+gEVJW4zQf4n1IFSilGBYsXJAjsjHPm9SEtkJi/Y79DD7cLmiZuKQ1eSEQIGwxpZ+xoRzhMUwiBfjTd9BSQo7sfWgIVaBUCgYEA/jPa2eH+Pk+9W4sPMECzU1mxJHaaaZu/soNbAvCj8pAUAeL0bIlJildwQUMGsZjSY1essLKYuWRznZucJPAcpeNDieZUoMuJQ02TLXbUrFivgbVLrgNNpgnTJ6PhkO2dChEDScmprzRtXthssQ0R1hGu2Ndboj8R3ctYcuSToz8CgYEA1KIBBzPt76/k+XqUeGTHiiYrn+M3ewsUKjCqiyIASo+wvp07D4goT9R/1PxFHrhO6w1xCkLnTrvrsn2FdhAjph+3KSADxWkA+1dwdVLJSBsGOBmUPg93/cO87OIW4UUNcEAxAscr1pnOlSwIVtngukcu2ZvLVYOSmTjh+Y/VRq8CgYEA31jUW0fzSPBq4bnm2HCgnwVZXT5QIaZU4hYDqyugAQmSaq3sMEjXspNs8ApiITyy7dIkywPAqHOYGcyVfubxZNevicQ0aysqKVZJkFKuPdeWLRMLsZL5cH+FZXjaQapkpmmYSAbF7kjUJ9dkeJORLlNPGvrtajqcChIWXqxzA6UCgYEAgLzBQJopQghN6tpTj/z8buEjYn+QNLgFcnHan7hrUrliulW5z3KRbZvr9YjzVGEkrt2JhjvxjdLrJjsnFc85tjfXjleq0Nf9Vy+ej/oakcSC5G5gOZ7l+EeNWCKe1a8YLtE+mz75hMnEsL5jIipyDHZSoD/aQSETHh+5eX6jE1MCgYAFYMthgztWvD4EllXp8CZVYavMf7VQBnVgcLcXhIsegqyfknfUpyRy01ZsR2fQqCpXMgZV5WEr/MBgypoTmVo1J1uV10gmqB53V6LZ4jtaA80Z/0+pgqJJSgZhq58Rtz5ed56WVOPpcX7VudS0OXSzjeFewGMRgZ4cgyyXG0az1w==";
            public const string notify_url = Ip + "/notify/Alipay_Notify";
        }
        /// <summary>
        /// 支付基本配置
        /// </summary>
        public static class Pay_Config
        {
            public const string title = "智慧洗衣--相伴有你";
            public const string body = "购买支付";

        }
    }
}