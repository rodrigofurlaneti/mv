using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TutorialRepositorio : NHibRepository<Tutorial>, ITutorialRepositorio
    {
        public TutorialRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
