using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using ApiInfox.Properties;
using Core.Exceptions;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [Authorize]
    [RoutePrefix("api/subGrupoProduto")]
    public class SubGrupoProdutoController : BaseController<SubGrupoProduto, ISubGrupoProdutoServico>
    {
        [HttpGet]
        [Route("subgrupos")]
        public List<SubGrupoProduto> GetGrupos()
        {
            return Servico.Buscar().ToList() ?? new List<SubGrupoProduto>();
        }

        [HttpGet]
        [Route("subGruposPorGrupo/{grupoId}")]
        public List<SubGrupoProduto> GetSubGruposPorGrupo(int grupoId)
        {
	        return Servico.GetSubGruposPorGrupo(grupoId).ToList();
        }
	}
}