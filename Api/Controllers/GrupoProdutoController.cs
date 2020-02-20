using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Api.Properties;
using Core.Exceptions;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/grupoProduto")]
    public class GrupoProdutoController : BaseController<GrupoProduto, IGrupoProdutoServico>
    {
        [HttpGet]
        [Route("grupos")]
        public List<GrupoProduto> GetGrupos()
        {
            return Servico.Buscar().ToList() ?? new List<GrupoProduto>();
        }

        [HttpGet]
        [Route("gruposPorSecao/{secaoId}")]
        public List<GrupoProduto> GetGruposPorSecao(int secaoId)
        {
	        return Servico.GetGruposPorSecao(secaoId).ToList();
        }

	    [HttpGet]
	    [Route("gruposPorCategoria/{categoriaId}")]
	    public List<GrupoProduto> GetGruposPorCategoria(int categoriaId)
	    {
		    return Servico.GetGruposPorCategoria(categoriaId).ToList();
	    }
	}
}