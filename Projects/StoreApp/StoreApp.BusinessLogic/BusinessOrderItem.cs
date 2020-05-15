using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.BusinessLogic
{
    public class BusinessOrderItem
    {
        public string OrderItemId { get; set; }
        public int Quantity { get; set; }

        public BusinessOrder Order { get; set; }
        public BusinessProduct Product { get; set; }
    }
}