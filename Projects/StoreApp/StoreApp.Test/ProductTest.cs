using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using StoreApp.BusinessLogic;
using StoreApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Test
{
    public class ProductTest
    {
        // Task<IEnumerable<BusinessProduct>> listProductsAsync(int locationId);
        // Task<BusinessProduct> getProductAsync(int productId, int locationId);
        
        [Fact]
        public async Task TestListInventories() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_inventory")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                var productA = new Product { ProductId = 1 };
                var productB = new Product { ProductId = 2 };
                var locationA = new Location { LocationId = 1 };
                var inventoryA = new Inventory {
                    Product = productA,
                    Location = locationA
                };
                var inventoryB = new Inventory {
                    Product = productB,
                    Location = locationA
                };
                bc.Add(productA);
                bc.Add(productB);
                bc.Add(locationA);
                bc.Add(inventoryA);
                bc.Add(inventoryB);

                await bc.SaveChangesAsync();

                var productsA = await repository.listInventoriesAsync();
                var productsB = await repository.listInventoriesAsync();
                productsA = productsA.Where(p => p.LocationId == 1);
                productsB = productsB.Where(p => p.LocationId == 2);

                Assert.Equal(2, productsA.Count());
                Assert.Empty(productsB);
            }
        }
    }
}