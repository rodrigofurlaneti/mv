using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class DescontoPessoaRepositorio : NHibRepository<DescontoPessoa>, IDescontoPessoaRepositorio
    {
        public DescontoPessoaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}