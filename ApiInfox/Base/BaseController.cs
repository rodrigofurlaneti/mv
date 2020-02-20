using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Core.Exceptions;
using Dominio.Base;
using Entidade.Base;
using Microsoft.Practices.ServiceLocation;

namespace ApiInfox.Base
{
    public class BaseController<TEntity, TServico> : ApiController where TEntity : IEntity where TServico : IBaseServico<TEntity>
    {

        #region Constructors / Destructors
        #endregion

        #region Private members
        private TServico _servico;

        private bool IsRequestAuthenticated()
        {
            if (Request.GetActionDescriptor().GetCustomAttributes<AllowAnonymousAttribute>().Any())
                return true;

            if (Attribute.IsDefined(GetType(), typeof(AllowAnonymousAttribute)))
                return true;

            return User.Identity.IsAuthenticated;
        }

        #endregion

        #region Protected members
        protected virtual TServico GetServico()
        {
            return ServiceLocator.Current.GetInstance<TServico>();
        }

        #endregion

        #region Public members

        public TServico Servico
        {
            get
            {
                if (_servico != null) return _servico;

                // Check if controller allow anonumous access.  If not allow the user has been autenticated
                if (!IsRequestAuthenticated())
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Could not resolve service class because the user is not authenticated."));
                //
                _servico = GetServico();
                return _servico;
            }
        }

        // GET api/values
        [HttpGet]
        [Route("")]
        public virtual IEnumerable<TEntity> Get()
        {
            return Servico.Buscar();
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id}")]
        public virtual TEntity Get(int id)
        {
            var entity = Servico.BuscarPorId(id);
            if (entity == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return entity;
        }

        
        // POST api/values
        [HttpPost]
        [Route("")]
        public virtual IHttpActionResult Post([FromBody] TEntity entity)
        {
            Servico.Salvar(entity);
            //
            return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString()) + entity.Id), entity.Id);
        }

        // PUT api/values/5
        [HttpPut]
        [Route("{id}")]
        public virtual void Put(int id, [FromBody] TEntity entity)
        {
            try
            {
                // Set id in object
                entity.Id = id;
                //
                Servico.Salvar(entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("{id}")]
        public virtual void Delete(int id)
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

        #endregion

    }
}