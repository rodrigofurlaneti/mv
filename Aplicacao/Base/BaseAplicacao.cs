using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominio.Base;
using Entidade.Base;
using Microsoft.Practices.ServiceLocation;

namespace Aplicacao.Base
{
    public class BaseAplicacao<TEntity, TServico> : IBaseAplicacao<TEntity> where TEntity : class, IEntity where TServico : IBaseServico<TEntity>
    {
        private IBaseServico<TEntity> _servico;
        public IBaseServico<TEntity> Servico => _servico ?? (_servico = GetServico());

        protected virtual TServico GetServico()
        {
            return ServiceLocator.Current.GetInstance<TServico>();
        }

        public IList<TEntity> Buscar()
        {
            return Servico.Buscar();
        }

        public TEntity BuscarPorId(int id)
        {
            return Servico.BuscarPorId(id);
        }

        public void Salvar(TEntity entity)
        {
            Servico.Salvar(entity);
        }

        public void Excluir(TEntity entity)
        {
            Servico.Excluir(entity);
        }

        public void ExcluirPorId(int id)
        {
            Servico.ExcluirPorId(id);
        }

        public int Contar()
        {
            return Servico.Contar();
        }

        public TEntity PrimeiroPor(Expression<Func<TEntity, bool>> query)
        {
            return Servico.PrimeiroPor(query);
        }

        public IList<TEntity> BuscarPor(Expression<Func<TEntity, bool>> query)
        {
            return Servico.BuscarPor(query);
        }

        public IList<TEntity> BuscarPorIntervalo(int firstResult, int maxResult, string entityOrderBy, string columnOrderBy)
        {
            return Servico.BuscarPorIntervalo(firstResult, maxResult, entityOrderBy, columnOrderBy);
        }

        public TEntity SalvarComRetorno(TEntity entity)
        {
            var id = Servico.SalvarComRetorno(entity);

            return BuscarPorId(id);
        }

    }
}