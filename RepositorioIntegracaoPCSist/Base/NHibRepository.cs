using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EntidadePcSist.Base;

namespace RepositorioIntegracaoPCSist.Base
{
    public abstract class NHibRepository<T> : IRepository<T>
        where T : class, IEntityPcSist
    {
        protected NHibRepository(NHibContext context)
        {
            Context = context;
            Session = Context.CreateNewSession();
        }

        protected NHibContext Context { get; }
        protected NHibSession Session { get; }

        #region IDisposable Members

        public void Dispose()
        {
            Session.Dispose();
        }

        #endregion

        #region IRepository<T> Members

        public void Commit()
        {
            Session.Commit();
        }

        public void Rollback()
        {
            Session.Rollback();
        }

        public void Clear()
        {
            Session.Clear();
        }

        public virtual void Save(T item)
        {
            Session.Save(item);
        }

        public virtual void Insert(T item)
        {
            Session.Insert(item);
        }

        public virtual void Update(T item)
        {
            Session.Update(item);
        }

        public virtual void Delete(T item)
        {
            Session.Delete(item);
        }

        public virtual T FirstBy(Expression<Func<T, bool>> query)
        {
            return Session.GetItemBy(query);
        }
        
        public virtual IList<T> List()
        {
            return Session.GetList<T>();
        }

        public virtual IList<T> ListByInterval(int firstResult, int maxResult, string entityOrderBy, string columnOrderBy)
        {
            return Session.GetList<T>(firstResult, maxResult, entityOrderBy, columnOrderBy);
        }
        
        public virtual IList<T> ListBy(Expression<Func<T, bool>> query)
        {
            return Session.GetListBy(query);
        }

        public virtual int Count()
        {
            return Session.GetCountList<T>();
        }

        public int GetCountList()
        {
            return Session.GetCountList<T>();
        }
        
        #endregion
    }
}