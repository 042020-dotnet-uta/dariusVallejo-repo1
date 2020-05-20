using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.BusinessLogic
{
    /// <summary>
    /// View model representation of a database order
    /// </summary>
    public class BusinessOrder
    {
        [Display(Name="order#")]
        public int OrderId { get; set; }

        [Display(Name="total")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float Total { get; set; }

        [Display(Name="order placed")]
        public string OrderDate { get; set; }

        public BusinessCustomer Customer { get; set; }
        public BusinessLocation Location { get; set; }
        public ISet<BusinessOrderItem> OrderItems { get; set; }
    }
}