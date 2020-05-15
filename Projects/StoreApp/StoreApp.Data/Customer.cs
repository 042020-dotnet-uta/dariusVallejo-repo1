using System;
using System.Collections.Generic;

namespace StoreApp.Data {
    public class Customer {
        public int CustomerId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
