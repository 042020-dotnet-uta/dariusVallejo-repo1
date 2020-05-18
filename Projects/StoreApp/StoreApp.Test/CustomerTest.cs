using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using StoreApp.BusinessLogic;
using StoreApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Test
{
    public class CustomerTest
    {
        [Fact]
        public void TestCustomerCreation() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_customer")
                            .Options;
            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);
                repository.createCustomerAsync(new BusinessCustomer { Username = "test" } );
                var customer = bc.Customers.Where(o => o.Username == "test").FirstOrDefault();
                Assert.Equal("test", customer.Username);
            }
        }

        [Fact]
        public async Task TestCustomerLogin() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_customer")
                            .Options;
            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);
                var customer = new BusinessCustomer { Username = "test2", Password = "test" };
                await repository.createCustomerAsync(customer);
                var login = await repository.loginCustomerAsync(customer);
                Assert.Equal(customer.Password, login.Password);
            }
        }

        [Fact]
        public async Task TestCustomerLoginFail() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_customer")
                            .Options;
            using (var bc = new BusinessContext(options)) {

                var repository = new Repository(bc);
                var customer = new BusinessCustomer { Username = "test3", Password = "test" };
                await repository.createCustomerAsync(customer);
                customer.Password = "wrong";
                var login = await repository.loginCustomerAsync(customer);
                Assert.Null(login);
            }
        }

        [Fact]
        public async Task TestGetCustomerByUsername() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_customer")
                            .Options;
            using (var bc = new BusinessContext(options)) {

                var repository = new Repository(bc);
                var customerA = new BusinessCustomer { Username = "test4" };
                await repository.createCustomerAsync(customerA);
                var customerSa = await repository.getCustomerByUsernameAsync(customerA.Username);
                Assert.Equal("test4", customerSa.Username);
            }
        }

        [Fact]
        public async Task TestGetCustomerByUsernameFail() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_customer")
                            .Options;
            using (var bc = new BusinessContext(options)) {

                var repository = new Repository(bc);
                var customerB = new BusinessCustomer { Username = "test5" };
                var customerSb = await repository.getCustomerByUsernameAsync(customerB.Username);
                var customerSn = await repository.getCustomerByUsernameAsync(null);
                Assert.Null(customerSb);
                Assert.Null(customerSn);
            }
        }

        [Fact]
        public async Task TestListCustomers() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_customer_list")
                            .Options;
            using (var bc = new BusinessContext(options)) {

                var repository = new Repository(bc);

                var customersSn = await repository.listCustomersAsync();
                var customerA = new BusinessCustomer { Username = "test8" };
                var customerB = new BusinessCustomer { Username = "test9" };
                await repository.createCustomerAsync(customerA);
                await repository.createCustomerAsync(customerB);
                var customersSa = await repository.listCustomersAsync();

                Assert.Equal(2, customersSa.Count());
                Assert.Empty(customersSn);
            }
        }

        [Fact]
        public async Task TestListCustomersEmpty() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_customer_empty")
                            .Options;
            using (var bc = new BusinessContext(options)) {

                var repository = new Repository(bc);

                var customersSn = await repository.listCustomersAsync();
                Assert.Empty(customersSn);
            }
        }
    }
}
