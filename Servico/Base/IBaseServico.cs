using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entidade.Base;

namespace Dominio.Base
{
    public interface IBaseServico<T> where T : IEntity
    {
        T BuscarPorId(int id);
        IList<T> Buscar();
        void Salvar(T item);
        int SalvarComRetorno(T entity);
        void Excluir(T item);
        void ExcluirPorId(int id);
        T PrimeiroPor(Expression<Func<T, bool>> query);
        IList<T> BuscarPor(Expression<Func<T, bool>> query);
        IList<T> BuscarPorIntervalo(int firstResult, int maxResult, string entityOrderBy, string columnOrderBy);
        int Contar();
        void Commit();
        void Clear();
        void ExcluirPor(Expression<Func<T, bool>> query);
    }
}