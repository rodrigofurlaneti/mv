using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface ITerminalCobrancaLojaServico : IBaseServico<TerminalCobrancaLoja>
    {

    }

    public class TerminalCobrancaLojaServico : BaseServico<TerminalCobrancaLoja, ITerminalCobrancaLojaRepositorio>, ITerminalCobrancaLojaServico
    {

    }
}
