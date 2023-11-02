using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
namespace WHMS.DAL.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        T Add(T entity);

        T Update(T entity);

        T GetById(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);


        void SaveChanges();
    }
}
