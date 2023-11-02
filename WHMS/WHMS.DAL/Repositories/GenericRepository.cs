using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WHMS.DAL.Data;
using WHMS.DAL.Repositories.Interfaces;

namespace WHMS.DAL.Repositories
{
    public class GenericRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _patternDbContext;
        public GenericRepository(ApplicationDbContext patternDbContext)
        {
            _patternDbContext = patternDbContext;
        }

        public T Add(T entity)
        {
            return _patternDbContext.Add(entity).Entity;
        }

        public IEnumerable<T> GetAll()
        {
            return _patternDbContext.Set<T>().ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _patternDbContext.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public T GetById(int id)
        {
            return _patternDbContext.Find<T>();
        }

        public void SaveChanges()
        {
            _patternDbContext.SaveChanges();
        }

        public T Update(T entity)
        {
            return _patternDbContext.Update(entity).Entity;
        }

       
    }
}
