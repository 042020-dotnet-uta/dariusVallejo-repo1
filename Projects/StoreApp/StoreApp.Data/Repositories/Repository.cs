using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using StoreApp.BusinessLogic;

namespace StoreApp.Data {
    public class Repository : IRepository {
        private readonly BusinessContext _context;

        public Repository(BusinessContext context) {
            _context = context;
        }

        public async Task<BusinessCustomer> createCustomerAsync(BusinessCustomer customer) {
            // TODO Meaningful exceptions that don't break
            if (await _context.Customers.AnyAsync(c => c.Username == customer.Username)) {
                  throw new InvalidOperationException("Already exists.");
            }
            Customer newCustomer = new Customer {
                Username = customer.Username,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Password = customer.Password
            };
            _context.Add(newCustomer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<BusinessCustomer> loginCustomerAsync(BusinessCustomer customer) {
            Customer databaseCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Username == customer.Username && c.Password == customer.Password);
            if (databaseCustomer != null) {
                return customer;
            } return null;
        }

        public async Task<BusinessCustomer> getCustomerByUsernameAsync(string username) {
            Customer databaseCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Username == username);
            if (databaseCustomer != null) {
                return new BusinessCustomer {
                    CustomerId = databaseCustomer.CustomerId,
                    Username = databaseCustomer.Username,
                    FirstName = databaseCustomer.FirstName,
                    LastName = databaseCustomer.LastName,
                    Email = databaseCustomer.Email
                };
            } return null;
        }

        public async Task<BusinessCustomer> getCustomerByIdAsync(int customerId) {
            Customer databaseCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (databaseCustomer != null) {
                return new BusinessCustomer {
                    CustomerId = databaseCustomer.CustomerId,
                    Username = databaseCustomer.Username,
                    FirstName = databaseCustomer.FirstName,
                    LastName = databaseCustomer.LastName,
                    Email = databaseCustomer.Email
                };
            } return null;
        }

        public async Task<IEnumerable<BusinessLocation>> listLocationsAsync() {
            List<Location> locations = await _context.Locations.ToListAsync();
            return locations.Select(databaseLocation => new BusinessLocation {
                LocationId = databaseLocation.LocationId,
                LocationName = databaseLocation.LocationName
            });
        }

        public async Task<IEnumerable<BusinessProduct>> listProductsAsync(int locationId) {
            var businessProducts = await (from p in _context.Products
                                                    join i in _context.Inventories
                                                    on p.ProductId equals i.Product.ProductId
                                                    select new {
                                                        Product = p,
                                                        Quantity = i.Quantity,
                                                        LocationId = i.Location.LocationId
                                                    }).Where(a => a.LocationId == locationId).ToListAsync();
            return businessProducts.Select(bp => new BusinessProduct {
                ProductId = bp.Product.ProductId,
                ProductName = bp.Product.ProductName,
                ProductPrice = bp.Product.ProductPrice,
                LocationId = locationId,
                Quantity = bp.Quantity,
            });
        }

        public async Task<BusinessProduct> getProductAsync(int productId, int locationId) {
            var result = await listProductsAsync(locationId);
            return result.Select(bp => new BusinessProduct {
                ProductId = bp.ProductId,
                ProductName = bp.ProductName,
                ProductPrice = bp.ProductPrice,
                LocationId = locationId,
                Quantity = bp.Quantity,
            }).Where(p => p.ProductId == productId && p.LocationId == p.LocationId).FirstOrDefault();
        }

        public async Task<IEnumerable<BusinessOrder>> listOrdersByCustomerAsync(int customerId) {
            // var result = await _context.Orders.Where(o => o.Customer.CustomerId == customerId).ToListAsync();
            var orders = await _context.Orders.Include(o => o.OrderItems).ToListAsync();
            return orders.Select(o => new BusinessOrder {
                OrderId = o.OrderId,
                // Total = 0,
                // OrderDate = 0,
                OrderItems = o.OrderItems.Select(oi => new BusinessOrderItem {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity
                }).ToHashSet()
                
                // Customer
                // Location
                // OrderItems = o.OrderItems.Select(oi => new BusinessOrderItem {
                //     OrderItemId = oi.OrderItemId,
                //     Quantity = oi.Quantity,

                //     // Order = oi.Order,
                //     Product = new BusinessProduct {
                //         ProductId = oi.Product.ProductId,
                //         ProductName = oi.Product.ProductName,
                //         ProductPrice = oi.Product.ProductPrice
                //     }
                // }).ToHashSet()
            });
        }

        public async Task<BusinessOrder> createOrderAsync(int customerId) {
            Customer customer = await _context.Customers.Where(c => c.CustomerId == customerId).FirstOrDefaultAsync();
            Order order = new Order {
                Customer = customer,
                OrderItems = new List<OrderItem>()
            };
            _context.Add(order);
            await _context.SaveChangesAsync();
            return new BusinessOrder();
        }

        public async Task<BusinessOrder> updateOrderAsync(BusinessOrder businessOrder, BusinessOrderItem businessOrderItem) {
            Order order = await _context.Orders.Include(o => o.OrderItems).Where(o => o.OrderId == businessOrder.OrderId).FirstOrDefaultAsync();
            OrderItem orderItem = new OrderItem {
                Quantity = businessOrderItem.Quantity,
                Order = order,
                Product = await _context.Products.Where(p => p.ProductId == businessOrderItem.Product.ProductId).FirstOrDefaultAsync(),
            };
            order.OrderItems.Add(orderItem);
            businessOrder.OrderItems.Add(businessOrderItem);
            _context.Add(orderItem);
            await _context.SaveChangesAsync();
            return businessOrder;
        }

        public async Task<BusinessOrder> submitOrderAsync(BusinessOrder businessOrder) {
            return null;
        }
    }
}