using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PessoaRepositorio : NHibRepository<Pessoa>, IPessoaRepositorio
    {
        public PessoaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
