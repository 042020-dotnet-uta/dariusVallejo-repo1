using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.BusinessLogic;
using StoreApp.Data;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace StoreApp.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BusinessContext _context;
        private readonly IRepository _repository;

        public CustomerController(BusinessContext context, IRepository repository)
        {
            _context = context;
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
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Customer/Login
        [HttpPost]
        [ValidateAntiForgeryToken] // ???
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
                if (databaseCustomer != null)
                {
                    HttpContext.Session.SetInt32("CustomerId", databaseCustomer.CustomerId);
                    HttpContext.Session.SetString("Username", databaseCustomer.Username);
                    return RedirectToAction("Index", "Home");
                } else {
                    ModelState.AddModelError("Password", "Username or password is incorrect.");
                    return View();
                }
            } return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("CustomerId") != null)
            {
                return RedirectToAction("Index", "");
            }
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Email,Password")] Customer customer)
        public async Task<IActionResult> Create(CustomerViewModel customerView)
        {
            if (ModelState.IsValid)
            {
                var businessCustomer = new BusinessCustomer
                {
                    Username = customerView.Username,
                    FirstName = customerView.FirstName,
                    LastName = customerView.LastName,
                    Email = customerView.Email,
                    Password = customerView.Password
                };

                await _repository.createCustomerAsync(businessCustomer);
                return RedirectToAction(nameof(Index));
            }
            return View(customerView);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Email,Password")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
