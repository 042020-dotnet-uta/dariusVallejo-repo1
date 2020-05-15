using System.Collections.Generic;

namespace StoreApp.Data
{
    public class Order
    {
        public string OrderId { get; set; }
        public float Total { get; set; }
        public string OrderDate { get; set; }

        public Customer Customer { get; set; }
        public Location Location { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}