using System;
using System.Collections.Generic;

namespace Reinforced.Stroke.Core.Demo.Data
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Customer()
        {
            Orders = new HashSet<Order>();
        }
    }
}
