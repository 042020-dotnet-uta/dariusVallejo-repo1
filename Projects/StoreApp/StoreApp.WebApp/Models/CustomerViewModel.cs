using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApp {

    /// <summary>
    /// View model representation of database customer
    /// </summary>
    public class CustomerViewModel {
        public int CustomerId { get; set; }

        [Required]
        [Display(Name="username")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [Display(Name="first name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [Display(Name="email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name="password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name="re-enter password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}