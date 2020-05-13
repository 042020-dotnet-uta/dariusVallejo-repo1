using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace StoreApp.Data {
    public class Repository : IRepository {
        private readonly BusinessContext _context;

        public Repository(BusinessContext context) {
            _context = context;
        }

        // TODO Change parameter input to generic CUSTOMER object
        public async Task createAsync(Customer entity) {
            // var entity = new Customer { FirstName = firstName, LastName = lastName };
            if (await _context.Customers.AnyAsync(c => c.FirstName == entity.FirstName)) {
                 throw new InvalidOperationException("Already exists.");
            }
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

    }
}