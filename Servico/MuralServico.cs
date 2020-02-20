using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IMuralServico : IBaseServico<Mural>
    {
    }

    public class MuralServico : BaseServico<Mural, IMuralRepositorio>, IMuralServico
    {  
    }
}