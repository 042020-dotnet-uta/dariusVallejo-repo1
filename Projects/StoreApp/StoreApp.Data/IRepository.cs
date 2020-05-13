using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApp.Data {
    public interface IRepository {
        public Task createAsync(Customer customer);
    }
}