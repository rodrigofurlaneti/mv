using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using Api.Base;
using Core.Exceptions;
using Dominio;
using Entidade;

namespace Api.Controllers
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

        [HttpGet]
        [Route("atualizargeolocalizacao")]
        public string geolocalizacao(string endereco)
        {
            try
            {
                string json = "";
                
                string address = endereco;
                string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyCMA4NQwhY6o9S6kEs7NlGzD4oM-JUg4ic&sensor=false", Uri.EscapeDataString(address));

                WebRequest request = WebRequest.Create(requestUri);
                WebResponse response = request.GetResponse();
                XDocument xdoc = XDocument.Load(response.GetResponseStream());

                XElement result = xdoc.Element("GeocodeResponse").Element("result");
                XElement locationElement = result.Element("geometry").Element("location");
                XElement lat = locationElement.Element("lat");
                XElement lng = locationElement.Element("lng");


                var latitude = lat.ToString().Replace("<lat>", "").Replace("</lat>", "").ToString();
                var longitude = lng.ToString().Replace("<lng>", "").Replace("</lng>", "").ToString();

                return json;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}