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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
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
            var businessCustomer = new BusinessCustomer
            {
                Username = customerView.Username,
                Password = customerView.Password
            };
            var databaseCustomer = await _repository.loginCustomerAsync(businessCustomer);
            if (databaseCustomer != null)
            {
                HttpContext.Session.SetString("Username", databaseCustomer.Username);
                return RedirectToAction("Index", "Location");
            } else {
                ModelState.AddModelError("Password", "Username or password is incorrect.");
                return View();
            }
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Username") != null)
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
                HttpContext.Session.SetString("Username", businessCustomer.Username);
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
