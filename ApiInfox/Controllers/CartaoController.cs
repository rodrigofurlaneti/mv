using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;
using Core.Exceptions;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [Authorize]
    [RoutePrefix("api/cartao")]
    public class CartaoController : ApiController
    {
        private readonly ICartaoServico _cartaoServico;

        public CartaoController(ICartaoServico cartaoServico)
        {
            _cartaoServico = cartaoServico;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Cartao entity)
        {
            _cartaoServico.ValidaESalva(entity);
            //
            return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString()) + entity.Id), entity.Id);
        }

        [HttpPost]
        [Route("{id}")]
        public void Update(int id, [FromBody] Cartao entity)
        {
            try
            {
                // Set id in object
                entity.Id = id;
                if (id > 0)
                {
                    var cartao = _cartaoServico.BuscarPorId(id);
                    if (entity.Numero.Contains("X"))
                        entity.Numero = cartao.Numero;
                    if (entity.Cvv.Contains("X"))
                        entity.Cvv = cartao.Cvv;
                }
                //
                _cartaoServico.ValidaESalva(entity);
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
                _cartaoServico.ExcluirPorId(id);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("pessoa/{id}")]
        public IEnumerable<Cartao> GetByPessoa(int id)
        {
            try
            {
                return _cartaoServico.GetByPessoa(id);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("pessoa/{id}/parcialbloq")]
        public IEnumerable<Cartao> GetCartoesParcialBloqPorPessoa(int id)
        {
            try
            {
                return _cartaoServico.GetCartoesParcialBloqPorPessoa(id);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}