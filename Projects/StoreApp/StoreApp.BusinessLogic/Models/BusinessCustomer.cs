using System.ComponentModel.DataAnnotations;

namespace StoreApp.BusinessLogic {
    
    /// <summary>
    /// View model representation of a database customer
    /// </summary>
    public class BusinessCustomer {
        public int CustomerId { get; set; }

        [Display(Name="username")]
        public string Username { get; set; }
        
        [Display(Name="first name")]
        public string FirstName { get; set; }

        [Display(Name="last name")]
        public string LastName { get; set; }

        [Display(Name="email")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}