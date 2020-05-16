using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Data
{
    /// <summary>
    /// A model representing a line item for a specific order
    /// </summary>
    public class OrderItem
    {
        // Keys / columns required to be public
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }

        // Foreign key(s)
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}