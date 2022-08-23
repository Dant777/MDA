using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDA.Restaraunt.Messages.Repository
{


    public interface IRepository<T> where T : class
    {
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(int id);

        public IEnumerable<T> Get();
        public T GetByid(int id);
    }
}
