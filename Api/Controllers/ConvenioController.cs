using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Api.Base;
using Api.Helpers;
using Dominio;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/convenio")]
    public class ConvenioController : BaseController<Convenio, IConvenioServico>
    {
        [HttpGet]
        [Route("PorNomeDocumentoOuCidade")]
        public IEnumerable<Convenio> GetPorNomeDocumentoOuCidade(int estado, int cidade, string bairro, string dadosPesquisa, int inicio, int quantidade)
        {
            if (quantidade < 0)
                quantidade = 0;

            var lojas = Servico.BuscaPor(estado, cidade, bairro, dadosPesquisa).Skip(inicio).Take(quantidade);
            
            return lojas.OrderBy(x => x.Descricao);
        }
        
        [HttpGet]
        [Route("{id}/planos")]
        public IEnumerable<PlanoVenda> GetPlanos(int id, int inicio, int quantidade)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<PlanoVendaServico>();
            var produtos = new List<PlanoVenda>();

            return produtoPrecoBusiness.BuscarPor(x => x.Convenio.Id == id 
                                                        && usuario.ListaUsuarioConvenio.Any(y => y.Convenio.Id == x.Convenio.Id)
                                                        && x.Status).Skip(inicio).Take(quantidade);
        }

        [HttpGet]
        [Route("planosPorUsuario")]
        public IEnumerable<PlanoVenda> GetPlanosPorUsuario()
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var usuarioServico = ServiceLocator.Current.GetInstance<UsuarioServico>();
            usuario = usuarioServico.BuscarPorId(usuario.Id);

            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<PlanoVendaServico>();
            var produtos = new List<PlanoVenda>();

            if (usuario.ListaUsuarioConvenio == null || !usuario.ListaUsuarioConvenio.Any())
                throw new Exception("Não foi possível localizar convenios para o usuário logado.");

            return produtoPrecoBusiness.BuscarPor(x => x.Status).Where(x => usuario.ListaUsuarioConvenio.Any(y => y.Convenio.Id == x.Convenio.Id)).ToList();
        }
    }
}