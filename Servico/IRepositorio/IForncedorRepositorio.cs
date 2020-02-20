using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IFornecedorRepositorio : IRepository<Fornecedor>
    {
        IList<Fornecedor> BuscarFornecedors();
    }
}