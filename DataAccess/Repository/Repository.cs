using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public bool Add(T entity)
        {
            dbSet.Add(entity);
            var res = _db.SaveChanges();
            return res > 0;
        }

        public T Get(Guid id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public bool Remove(Guid id) => Remove(dbSet.Find(id));

        public bool Remove(T entity)
        {
            dbSet.Remove(entity);
            var res = _db.SaveChanges();
            return res > 0;
        }

        public bool RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
            var res = _db.SaveChanges();
            return res > 0;
        }
    }
}
