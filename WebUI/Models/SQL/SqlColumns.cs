using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Web;

namespace Inv.WebUI.Models
{
    public class SqlColumns
    {
        public int object_id { get; set; }
        public string name { get; set; }
        public int column_id { get; set; }
        public int system_type_id { get; set; }
        public bool is_nullable { get; set; }
    }
}
