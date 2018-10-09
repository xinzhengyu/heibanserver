using Dhand.Basic;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace stem.Models.Basic
{
    //redis连接器
    //封装了redis配置，setHash(),getHash(),deleteHash()三个方法
    public static class Redis
    {
        //redis连接封装
        private static string constr = Dhand.Basic.Config.Redis_Config.constr;
        private static Lazy<ConnectionMultiplexer> lazyConnection = null;

        public static ConnectionMultiplexer redis
        {
            get
            {
                if (lazyConnection == null)
                {
                    lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                    {
                        return ConnectionMultiplexer.Connect(constr);
                    });
                }
                return lazyConnection.Value;
            }
        }
        public static int getUserId(string uid)
        {
            var jiemi = Encrypt.DesDecrypt(uid, "heiban");
            var y= Regex.Split(jiemi, "asd", RegexOptions.IgnoreCase);
            return int.Parse(y[0]);
        }
        public static string getHash(string table, string key)
        {
            IDatabase db = redis.GetDatabase();
            string code = db.HashGet(table, key);
            return code;
        }

        public static bool setHash(string table, string key, string value)
        {
            IDatabase db = redis.GetDatabase();
            return db.HashSet(table, key, value);
        }

        public static bool deleteHash(string table, string key)
        {
            IDatabase db = redis.GetDatabase();
            return db.HashDelete(table, key);
        }

        public static bool deleteAll(string table)
        {
            IDatabase db = redis.GetDatabase();
            return db.KeyDelete(table);
        }
    }
}