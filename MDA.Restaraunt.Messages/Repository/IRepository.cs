using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDA.Restaraunt.Messages.Repository
{

    public interface IRepository<T> where T : class
    {
        public Task<bool> AddAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> DeleteByIdAsync(int id);
        public Task<bool> DeleteByOrderIdAsync(Guid orderId);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<T> GetByOrderIdAsync(Guid orderId);
    }
}
