using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface ILojaRepositorio : IRepository<Loja>
    {
        IList<Loja> BuscarLojas(string classificacao = "");
        IList<Loja> BuscarLojasPorNome(string nome = "");
        IList<Loja> BuscarLojasPorTipoPorNome(string nomeTipo = "");
        IList<Loja> BuscaLojasPor(int estado, int cidade, string bairro, string dadosPesquisa = "");
    }
}