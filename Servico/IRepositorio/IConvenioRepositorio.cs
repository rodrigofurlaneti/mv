using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IConvenioRepositorio : IRepository<Convenio>
    {
        IList<Convenio> BuscaPor(int estado, int cidade, string bairro, string dadosPesquisa = "");
    }
}
