using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApp.BusinessLogic {
    public interface IRepository {
        Task<BusinessCustomer> createCustomer(BusinessCustomer customer);
        Task<BusinessCustomer> loginCustomer(BusinessCustomer customer);
    }
}