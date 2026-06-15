using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalClient.Models
{
    public class InventoryItem
    {
        public int ItemId { get; set; }
        public int Name { get; set; }
        public int Category { get; set; }
        public int QtyInStock { get; set; }
        public int ReorderThreshold { get; set; }

    }
}
