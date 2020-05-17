using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApp.BusinessLogic {
    public interface IRepository {

        Task<BusinessCustomer> createCustomerAsync(BusinessCustomer customer);
        Task<BusinessCustomer> loginCustomerAsync(BusinessCustomer customer);
        Task<BusinessCustomer> getCustomerByIdAsync(int customerId);
        Task<BusinessCustomer> getCustomerByUsernameAsync(string username);

        Task<IEnumerable<BusinessLocation>> listLocationsAsync();

        Task<IEnumerable<BusinessProduct>> listProductsAsync(int locationId);
        Task<BusinessProduct> getProductAsync(int productId, int locationId);

        Task<BusinessOrder> createOrderAsync(int customerId, int locationId);
        Task<BusinessOrder> updateOrderAsync(BusinessOrder order, BusinessOrderItem orderItem);
        Task<BusinessOrder> submitOrderAsync(BusinessOrder order);
        Task<IEnumerable<BusinessOrder>> listOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<BusinessOrder>> listOrdersAsync();

        Task<IEnumerable<BusinessOrderItem>> listOrderItemsAsync(int customerid, int orderId);
        Task<BusinessOrderItem> updateInventoryAsync(int locationId, BusinessOrderItem orderItem);
    }
}