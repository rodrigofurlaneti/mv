using Dominio.IRepositorio;
using Entidade;
using NHibernate.Criterion;
using Repositorio.Base;
using System.Linq;

namespace Repositorio
{
    public class CupomRepositorio : NHibRepository<Cupom>, ICupomRepositorio
    {
        public CupomRepositorio(NHibContext context)
            : base(context)
        {
        }

        public Cupom BuscarCupom(string cupom)
        {
            return Session.CreateCriteria<Cupom>()
                .Add(Restrictions.Eq("CodigoCupom", cupom))
                .List<Cupom>().FirstOrDefault() ?? new Cupom();
        }
    }
}