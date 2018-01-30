using System.Collections.Generic;

namespace Reinforced.Stroke.Core.Demo.Data
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Subtotal { get; set; }

        public ICollection<Item> Items { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public Order()
        {
            Items = new HashSet<Item>();
        }
    }
}