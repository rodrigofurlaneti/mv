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
	[RoutePrefix("api/secaoProduto")]
	public class SecaoProdutoController : BaseController<SecaoProduto, ISecaoProdutoServico>
	{
		[HttpGet]
		[Route("secoesPorCategoria/{categoriaId}")]
		public List<SecaoProduto> GetSecoesPorCategoria(int categoriaId)
		{
			return Servico.GetSecoesPorCategoria(categoriaId).ToList();
		}

		[HttpGet]
		[Route("secoes")]
		public List<SecaoProduto> GetSecoes()
		{
			return Servico.Buscar().ToList();
		}

        [HttpGet]
        [Route("secoesPorDepartamento/{departamento}")]
        public List<SecaoProduto> GetSecoesPorDepartamento(int departamento)
        {
            return Servico.BuscarPor(x => x.Departamento.Id == departamento)?.ToList() ?? new List<SecaoProduto>();
        }
    }
}