//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class lesson_comment
    {
        public int id { get; set; }
        public string desc { get; set; }
        public string imglist { get; set; }
        public bool is_niming { get; set; }
        public Nullable<int> uid { get; set; }
        public int lesson_id { get; set; }
        public int pre_id { get; set; }
        public Nullable<int> order_id { get; set; }
        public Nullable<int> toid { get; set; }
        public Nullable<System.DateTime> ins_date { get; set; }
        public Nullable<double> first_star { get; set; }
        public Nullable<double> second_star { get; set; }
        public Nullable<double> third_star { get; set; }
        public string uname { get; set; }
        public string toname { get; set; }
    }
}
