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
        public string Name { get; set; }
        public string Category { get; set; }
        public int QtyInStock { get; set; }
        public int ReorderThreshold { get; set; }

        public bool IsLowStock
        {
            get { return QtyInStock <= ReorderThreshold; }
        }

    }
}
