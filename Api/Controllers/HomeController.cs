using System.Web.Http;
using Api.Helpers;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        private readonly IUsuarioServico _usuarioServico;

        public HomeController(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("test")]
        public Usuario Test()
        {
            return LoggedUser.GetLoggedUser();
        }

        [HttpGet]
        [Route("test2")]
        public string Test2()
        {
            return "teste2";
        }

        [HttpGet]
        [Route("test3")]
        public int Test3()
        {
            //var usuarioApp = ServiceLocator.Current.GetInstance<IUsuarioAplicacao>();
            return _usuarioServico.Contar();
        }
    }
}