using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Exceptions;
using Dominio.IRepositorio.Base;
using Entidade.Base;
using Microsoft.Practices.ServiceLocation;

namespace Dominio.Base
{
    public class BaseServico<TEntity, TRepositorio> 
        : IBaseServico<TEntity> 
        where TEntity : class, IEntity 
        where TRepositorio : IRepository<TEntity>
    {
        private IRepository<TEntity> _repositorio;
        public IRepository<TEntity> Repositorio => _repositorio ?? (_repositorio = GetRepositorio());

        protected virtual TRepositorio GetRepositorio()
        {
            return ServiceLocator.Current.GetInstance<TRepositorio>();
        }

        public IList<TEntity> Buscar()
        {
            try
            {
                return Repositorio.List();
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public TEntity BuscarPorId(int id)
        {
            try
            {
                return Repositorio.GetById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public void Salvar(TEntity entity)
        {
            try
            {
                Repositorio.Save(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public int SalvarComRetorno(TEntity entity)
        {
            try
            {
                return Repositorio.SaveAndReturn(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public void Excluir(TEntity entity)
        {
            try
            {
                Repositorio.Delete(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public void ExcluirPorId(int id)
        {
            try
            {
                Repositorio.DeleteById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public int Contar()
        {
            try
            {
                return Repositorio.Count();
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public void Commit()
        {
            Repositorio.Commit();
        }

        public void Clear()
        {
            Repositorio.Clear();
        }

        public TEntity PrimeiroPor(Expression<Func<TEntity, bool>> query)
        {
            try
            {
                return Repositorio.FirstBy(query);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public IList<TEntity> BuscarPor(Expression<Func<TEntity, bool>> query)
        {
            try
            {
                return Repositorio.ListBy(query);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public IList<TEntity> BuscarPorIntervalo(int firstResult, int maxResult, string entityOrderBy, string columnOrderBy)
        {
            try
            {
                return Repositorio.ListByInterval(firstResult, maxResult, entityOrderBy, columnOrderBy);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }

        public void ExcluirPor(Expression<Func<TEntity, bool>> query)
        {
            try
            {
                Repositorio.DeleteBy(query);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException(ex.Message, ex);
            }
        }
    }
}