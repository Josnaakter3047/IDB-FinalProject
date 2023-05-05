using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(Guid id);
        IEnumerable<T> GetAll();
        bool Add(T entity);
        bool Remove(Guid id);
        bool Remove(T entiry);
        bool RemoveRange(IEnumerable<T> entity);
    }
}
