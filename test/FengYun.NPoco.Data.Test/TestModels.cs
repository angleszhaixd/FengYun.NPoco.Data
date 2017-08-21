using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPoco;
using FengYun.NPoco.Data;

namespace FengYun.NPoco.Data.Test
{
    
    [TableName("Users")]
    public class User: Entity<int>
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime birthday { get; set; }
        public string Remark { get; set; }
    }
    [TableName("NewsInfo")]
    public class News : Entity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public bool? Flag { get; set; } 
        public string Remark { get; set; }
    }
}