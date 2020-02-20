using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EntidadePcSist.Base;

namespace RepositorioIntegracaoPCSist.Base
{
    public interface IRepository<T> where T : class, IEntityPcSist
    {
        void Commit();
        void Rollback();
        void Clear();

        void Save(T item);
        void Insert(T item);
        void Update(T item);
        void Delete(T item);
        void Dispose();

        T FirstBy(Expression<Func<T, bool>> query);
        
        IList<T> List();
        IList<T> ListBy(Expression<Func<T, bool>> query);
        IList<T> ListByInterval(int firstResult, int maxResult, string entityOrderBy, string columnOrderBy);

        int Count();
    }
}
