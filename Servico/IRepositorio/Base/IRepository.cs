using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entidade.Base;
using NHibernate;

namespace Dominio.IRepositorio.Base
{
    public interface IRepository<T> where T : class, IEntity
    {
        void Commit();
        void Rollback();
        void Clear();

        void Save(T item);
        int SaveAndReturn(T item);
        void Insert(T item);
        void Update(T item);
        void Delete(T item);
        void DeleteById(int id);
        void Dispose();

        T FirstBy(Expression<Func<T, bool>> query);
        T GetById(int id);

        IList<T> List();
        IList<T> ListBy(Expression<Func<T, bool>> query);
        IList<T> ListByInterval(int firstResult, int maxResult, string entityOrderBy, string columnOrderBy);

        int Count();

        ITransaction SetupNewTransaction();
        void Save(IList<T> itens);
        void DeleteBy(Expression<Func<T, bool>> query);
        void DeleteByQuery(string query);
    }
}
