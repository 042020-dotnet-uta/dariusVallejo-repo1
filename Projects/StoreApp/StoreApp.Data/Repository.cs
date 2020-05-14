using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace StoreApp.Data {
    public class Repository : IRepository {
        private readonly BusinessContext _context;

        public Repository(BusinessContext context) {
            _context = context;
        }

        // TODO Change parameter input to generic object
        public async Task createAsync(Customer entity) {
            // TODO Meaningful exceptions that don't break
            if (await _context.Customers.AnyAsync(c => c.Username == entity.Username)) {
                  throw new InvalidOperationException("Already exists.");
            }
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer> findAsync(Customer entity) {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Username == entity.Username && c.Password == entity.Password);
        }
    }
}