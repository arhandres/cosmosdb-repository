using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.DataAccess
{
    public abstract class BaseRepository<T> where T : class
    {
        protected abstract T GetFirstOrDefault(Func<T, bool> predicate);
        protected abstract List<T> GetAll(Func<T, bool> predicate = null);
        protected abstract bool Upsert(T entity);
        protected abstract bool Delete(T entity);
    }
}
