using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.BusinessLogic;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace StoreApp.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public CustomerController(ILogger<CustomerController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: Customer
        public async Task<IActionResult> Index(string searchString)
        {
            string username = HttpContext.Session.GetString("Username");
            if (username == "admin") {
                var customers = await _repository.listCustomersAsync();
                customers = customers.Where(c => c.Username != username);
                if (!String.IsNullOrWhiteSpace(searchString)) {
                    searchString = searchString.Trim().ToUpper();
                    customers = customers.Where(c => new String(c.FirstName + c.LastName).ToUpper().Contains(searchString));
                }
                var customerViews = customers.Select(c => new CustomerViewModel {
                    CustomerId = c.CustomerId,
                    Username = c.Username,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email
                });
                return View(customerViews);
            } else {
                _logger.LogCritical("Admin permissions required");
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Customer/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("CustomerId") != null) {
                return RedirectToAction("Index", "Home");
            } return View();
        }

        // POST: Customer/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(CustomerViewModel customerView)
        {
            var sessionValue = HttpContext.Session.GetInt32("CustomerId");
            if (sessionValue == null) {
                var businessCustomer = new BusinessCustomer
                {
                    Username = customerView.Username,
                    Password = customerView.Password
                };
                var databaseCustomer = await _repository.loginCustomerAsync(businessCustomer);
                if (databaseCustomer != null) {
                    HttpContext.Session.SetInt32("CustomerId", databaseCustomer.CustomerId);
                    HttpContext.Session.SetString("Username", databaseCustomer.Username);
                    _logger.LogInformation("Session created for {0}", customerView.Username);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Password", "Invalid username or password.");
                _logger.LogWarning("Invalid username or password.");
                return View();
            } return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout() {
            var username = HttpContext.Session.GetString("Username");
            if (username != null) {
                HttpContext.Session.Clear();
                _logger.LogInformation("Session ended for {0}", username);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Customer/Create
        public IActionResult Create() {
            if (HttpContext.Session.GetInt32("CustomerId") != null) {
                return RedirectToAction("Index", "Home");
            } return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel customerView) {
            if (ModelState.IsValid) {
                var businessCustomer = new BusinessCustomer {
                    Username = customerView.Username,
                    FirstName = customerView.FirstName,
                    LastName = customerView.LastName,
                    Email = customerView.Email,
                    Password = customerView.Password
                };
                try {
                    await _repository.createCustomerAsync(businessCustomer);
                    _logger.LogInformation("Account created for {0}", customerView.Username);
                    return RedirectToAction("Login");
                } catch (Exception exception)  {
                    ModelState.AddModelError("Username", exception.Message);
                    _logger.LogWarning(exception.Message);
                }
            } return View(customerView);
        }
    }
}
