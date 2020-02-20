using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiInfox.Base;
using Core.Exceptions;
using System.Net;
using System.Web;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [Authorize]
    [RoutePrefix("api/cupom")]
    public class CupomController : BaseController<Cupom, ICupomServico>
    {
        [HttpPost]
        [Route("save/{id}")]
        public virtual IHttpActionResult Save(int id, [FromBody] Cupom entity)
        {
            try
            {
                entity.Id = id;
                Servico.Salvar(entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString()) + entity.Id), entity.Id);
        }

        [HttpPost]
        [Route("remove/{id}")]
        public virtual void Remove(int id, [FromBody] Cupom entity)
        {
            try
            {
                Servico.ExcluirPorId(id);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        
        [HttpGet]
        [Route("{id}")]
        public override Cupom Get(int id)
        {
            var entity = Servico.BuscarPorId(id);
            if (entity == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return entity;
        }

        [HttpPost]
        [Route("list")]
        public virtual IList<Cupom> List()
        {
            try
            {
                return Servico.Buscar();
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}