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
    [RoutePrefix("api/categoriaProduto")]
    public class CategoriaProdutoController : BaseController<CategoriaProduto, ICategoriaProdutoServico>
    {
        [HttpGet]
        [Route("categorias")]
        public List<CategoriaProduto> GetCategorias()
        {
	        return Servico.GetCategorias().ToList();
        }

	    [HttpGet]
	    [Route("categoriasPorSecao/{secaoId}")]
	    public List<CategoriaProduto> GetCategorias(int secaoId)
	    {
		    return Servico.GetCategoriasPorSecao(secaoId).ToList();
	    }
	}
}