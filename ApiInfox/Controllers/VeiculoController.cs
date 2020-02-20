using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ApiInfox.Base;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/veiculo")]
    public class VeiculoController : BaseController<Veiculo, IVeiculoServico>
    {
        private readonly IMarcaServico _marcaServico;

        public VeiculoController(IMarcaServico marcaServico)
        {
            _marcaServico = marcaServico;
        }

        [HttpGet]
        [Route("veiculosPorUsuario/{usuarioId}")]
        public IEnumerable<Veiculo> BuscarVeiculosPorUsuario(int usuarioId)
        {
            return Servico.BuscarPor(x => (x.Proprietario.Id == usuarioId || x.OutrosProprietarios.Count(y => y.Id == usuarioId) > 0)).Where(x => x.GetType() == typeof(Veiculo));
        }

        [HttpGet]
        [Route("marcas")]
        public IEnumerable<Marca> BuscarMarcas()
        {
            return _marcaServico.Buscar();
        }
    }
}