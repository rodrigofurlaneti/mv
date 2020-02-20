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