using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApp.BusinessLogic {
    public interface IRepository {
        Task<BusinessCustomer> createCustomerAsync(BusinessCustomer customer);
        Task<BusinessCustomer> loginCustomerAsync(BusinessCustomer customer);

        Task<IEnumerable<BusinessLocation>> listLocationsAsync();

        Task<IEnumerable<BusinessProduct>> listProductsAsync(BusinessLocation location);
        // Task<BusinessProduct> getProductAsync(int id);
    }
}