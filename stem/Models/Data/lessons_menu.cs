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
    
    public partial class lessons_menu
    {
        public int id { get; set; }
        public Nullable<int> lesson_id { get; set; }
        public Nullable<int> pre_id { get; set; }
        public string title { get; set; }
        public Nullable<int> orderby { get; set; }
        public Nullable<System.DateTime> ins_date { get; set; }
        public Nullable<bool> is_free { get; set; }
        public Nullable<double> price { get; set; }
        public Nullable<int> product_id { get; set; }
        public Nullable<int> time { get; set; }
        public string type { get; set; }
    }
}
