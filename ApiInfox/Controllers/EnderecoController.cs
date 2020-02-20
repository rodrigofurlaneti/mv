using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;
using ApiInfox.Base;
using Core.Exceptions;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [Authorize]
    [RoutePrefix("api/endereco")]
    public class EnderecoController : BaseController<Endereco, IEnderecoServico>
    {
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Endereco entity)
        {
            Servico.Salvar(entity);
            return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString()) + entity.Id), entity.Id);
        }

        [HttpPost]
        [Route("{id}")]
        public void Update(int id, [FromBody] Endereco entity)
        {
            try
            {
                entity.Pessoa.Id = id;
                Servico.Salvar(entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("deletar/{id}")]
        public void Delete(int id)
        {
            try
            {
                var entity = Servico.BuscarPorId(id);
                Servico.ExcluirPorId(id);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("pessoa/{id}")]
        public IEnumerable<Endereco> GetByPessoa(int id)
        {
            try
            {
                return Servico.GetByPessoa(id);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}