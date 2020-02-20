﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entidade.Base;

namespace Aplicacao.Base
{
    public interface IBaseAplicacao<T> where T : IEntity
    {
        T BuscarPorId(int id);
        IList<T> Buscar();
        void Salvar(T entity);
        T SalvarComRetorno(T entity);
        void Excluir(T entity);
        void ExcluirPorId(int id);
        T PrimeiroPor(Expression<Func<T, bool>> query);
        IList<T> BuscarPor(Expression<Func<T, bool>> query);
        IList<T> BuscarPorIntervalo(int firstResult, int maxResult, string entityOrderBy, string columnOrderBy);
        int Contar();
    }
}