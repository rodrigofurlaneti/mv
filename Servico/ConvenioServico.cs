using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface IConvenioServico : IBaseServico<Convenio>
    {
        IList<Convenio> BuscaPor(int estado, int cidade, string bairro, string dadosPesquisa = "");
    }

    public class ConvenioServico : BaseServico<Convenio, IConvenioRepositorio>, IConvenioServico
    {
        private readonly IConvenioRepositorio _convenioRepositorio;

        public ConvenioServico(IConvenioRepositorio convenioRespositorio)
        {
            _convenioRepositorio = convenioRespositorio;
        }

        public IList<Convenio> BuscaPor(int estado, int cidade, string bairro, string dadosPesquisa = "")
        {
            return _convenioRepositorio.BuscaPor(estado, cidade, bairro, dadosPesquisa);
        }
    }
}
