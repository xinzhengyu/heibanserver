﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace stem.Models.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class stemEntities : DbContext
    {
        public stemEntities()
            : base("name=stemEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<user> user { get; set; }
        public virtual DbSet<user_login> user_login { get; set; }
        public virtual DbSet<tables_common> tables_common { get; set; }
        public virtual DbSet<user_auth> user_auth { get; set; }
        public virtual DbSet<user_auth_type> user_auth_type { get; set; }
        public virtual DbSet<language_type> language_type { get; set; }
        public virtual DbSet<lesson> lesson { get; set; }
        public virtual DbSet<product_status> product_status { get; set; }
        public virtual DbSet<product_type> product_type { get; set; }
        public virtual DbSet<shop> shop { get; set; }
        public virtual DbSet<lessson_collect> lessson_collect { get; set; }
        public virtual DbSet<product_collect> product_collect { get; set; }
        public virtual DbSet<product_comment> product_comment { get; set; }
        public virtual DbSet<product_size> product_size { get; set; }
        public virtual DbSet<order> order { get; set; }
        public virtual DbSet<order_every_status> order_every_status { get; set; }
        public virtual DbSet<order_status> order_status { get; set; }
        public virtual DbSet<product_shop_category> product_shop_category { get; set; }
        public virtual DbSet<system_banner> system_banner { get; set; }
        public virtual DbSet<lessons_menu> lessons_menu { get; set; }
        public virtual DbSet<shop_status> shop_status { get; set; }
        public virtual DbSet<shop_teachers> shop_teachers { get; set; }
        public virtual DbSet<shop_type> shop_type { get; set; }
        public virtual DbSet<lesson_video> lesson_video { get; set; }
        public virtual DbSet<shops_teachers> shops_teachers { get; set; }
        public virtual DbSet<lesson_comment> lesson_comment { get; set; }
        public virtual DbSet<shops_banners> shops_banners { get; set; }
        public virtual DbSet<shops_citys> shops_citys { get; set; }
        public virtual DbSet<product> product { get; set; }
        public virtual DbSet<chat_list> chat_list { get; set; }
        public virtual DbSet<chat_msg> chat_msg { get; set; }
        public virtual DbSet<lesson_copy> lesson_copy { get; set; }
        public virtual DbSet<lesson_user> lesson_user { get; set; }
    }
}
