using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Providers
{
    public interface IClubeProvider
    {
        bool TemAcesso(Usuario usuario);
    }
}
