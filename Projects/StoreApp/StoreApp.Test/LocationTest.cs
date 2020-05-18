using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using StoreApp.BusinessLogic;
using StoreApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Test
{
    public class LocationTest
    {
        [Fact]
        public async Task TestListLocations() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_location_list")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                bc.Add(new Location { LocationName = "test" });
                bc.Add(new Location { LocationName = "test2" });
                await bc.SaveChangesAsync();

                var locations = await repository.listLocationsAsync();

                Assert.Equal(2, locations.Count());
            }
        }

        [Fact]
        public async Task TestListLocationsEmpty() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_location_empty")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                var locations = await repository.listLocationsAsync();
                Assert.Empty(locations);
            }
        }
    }
}