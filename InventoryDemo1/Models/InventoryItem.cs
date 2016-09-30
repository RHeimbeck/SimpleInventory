using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryDemo1.Models
{
    public class InventoryItem
    {
        public string label { get; set; }
        public DateTime expiration { get; set; }
        public string type { get; set; }
    }
}