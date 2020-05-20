using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.BusinessLogic
{
    /// <summary>
    /// View model representation of a database orderitem
    /// </summary>
    public class BusinessOrderItem
    {
        [Display(Name="line item#")]
        public int OrderItemId { get; set; }

        [Display(Name="quantity")]
        public int Quantity { get; set; }

        public BusinessOrder Order { get; set; }
        public BusinessProduct Product { get; set; }
    }
}