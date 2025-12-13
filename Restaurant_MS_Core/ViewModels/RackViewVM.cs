using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class RackViewVM
    {
        public int RackId { get; set; }
        public string Name { get; set; }
        public ICollection<ItemsVM> Items { get; set; }
    }
}