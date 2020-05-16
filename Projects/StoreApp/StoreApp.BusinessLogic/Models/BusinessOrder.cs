using System.Collections.Generic;

namespace StoreApp.BusinessLogic
{
    public class BusinessOrder
    {
        public int OrderId { get; set; }
        public float Total { get; set; }
        public string OrderDate { get; set; }

        public BusinessCustomer Customer { get; set; }
        public BusinessLocation Location { get; set; }
        public ISet<BusinessOrderItem> OrderItems { get; set; }
    }
}