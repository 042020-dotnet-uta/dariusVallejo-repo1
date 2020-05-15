using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApp.BusinessLogic {
    public interface IRepository {
        Task<BusinessCustomer> createCustomerAsync(BusinessCustomer customer);
        Task<BusinessCustomer> loginCustomerAsync(BusinessCustomer customer);

        Task<IEnumerable<BusinessLocation>> listLocationsAsync();

        Task<IEnumerable<BusinessProduct>> listProductsAsync(int locationId);
        Task<BusinessProduct> getProductAsync(int productId, int locationId);
    }
}