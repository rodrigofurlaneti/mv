using Dominio.Providers;
using Entidade;
using System;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/clube")]
    public class ClubeController : ApiController
    {
        private readonly IClubeProvider clubeProvider;

        public ClubeController(IClubeProvider clubeProvider)
        {
            this.clubeProvider = clubeProvider;
        }

        [HttpGet]
        [Route("acesso")]
        public bool PossuiAcesso(int usuarioId)
        {
            if (usuarioId == 0)
                throw new Exception("Não foi possível obter o usuário logado.");

            var usuario = new Usuario { Id = usuarioId };

            return clubeProvider.TemAcesso(usuario);
        }
    }
}
