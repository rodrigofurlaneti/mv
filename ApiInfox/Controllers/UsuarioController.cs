using System;
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
    [AllowAnonymous]
    [RoutePrefix("api/usuario")]
    public class UsuarioController : BaseController<Usuario, IUsuarioServico>
    {
        private readonly IPessoaServico _pessoaServico;
        private readonly IEnderecoServico _enderecoServico;
        private readonly IDocumentoServico _documentoServico;
        private readonly IContatoServico _contatoServico;
        private readonly ICartaoServico _cartaoServico;

        public UsuarioController(IPessoaServico pessoaServico, IEnderecoServico enderecoServico, IDocumentoServico documentoServico, IContatoServico contatoServico, ICartaoServico cartaoServico)
        {
            _pessoaServico = pessoaServico;
            _enderecoServico = enderecoServico;
            _documentoServico = documentoServico;
            _contatoServico = contatoServico;
            _cartaoServico = cartaoServico;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgotpassword")]
        public void ForgotPassword([FromBody]string login)
        {
            if(string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));

            Servico.RecuperarSenha(login, Resources.RecuperarSenha);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("account/register")]
        public IHttpActionResult Register(RegisterModel registerModel)
        {
            var pessoa = _pessoaServico.BuscarPorId(registerModel.Pessoa.Id);
            var usuario = Servico.BuscarPorId(registerModel.Id);
            var senha = usuario != null && string.IsNullOrEmpty(registerModel.Senha) && string.IsNullOrEmpty(registerModel.FacebookId) ? usuario.Senha : registerModel.Senha;

            if (registerModel.Id <= 0 && pessoa == null)
            {
                pessoa = registerModel.Pessoa;
                pessoa.Id = _pessoaServico.SalvarComRetorno(registerModel.Pessoa);

                Servico.Register(pessoa, senha, registerModel.Id, registerModel.FacebookId);
            }
            else 
            {
                _pessoaServico.ValidaPessoa(pessoa, registerModel.Id);

                // Merge cartao
                var cartoes = _cartaoServico.BuscarPor(x => x.Pessoa.Id.Equals(pessoa.Id));
                pessoa.Cartoes = cartoes;

                // Merge endereco
                var enderecos = _enderecoServico.BuscarPor(x => x.Pessoa.Id.Equals(pessoa.Id));
                pessoa.EnderecosEntrega = enderecos;

                _pessoaServico.Salvar(pessoa);

                foreach (var documento in pessoa.Documentos)
                {
                    documento.Pessoa = pessoa;
                    _documentoServico.Salvar(documento);
                }
                foreach (var contato in pessoa.Contatos)
                {
                    contato.Contato.Pessoa = pessoa;
                    _contatoServico.Salvar(contato.Contato);
                }

                Servico.Register(pessoa, senha, registerModel.Id, registerModel.FacebookId);
            }

            return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString().Replace(Request.RequestUri.AbsolutePath, "/api/client")) + registerModel.Pessoa.Id), registerModel.Pessoa.Id);
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("facebook/{email}/{facebookId}")]
        public Usuario LoginFacebook(string email, string facebookId)
        {
            try
            {
                return Servico.LoginFacebook(email, facebookId);
            }
            catch (BusinessRuleException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = ex.Message });
            }
        }

        [AllowAnonymous]
	    [HttpPost]
	    [Route("facebookLogin")] //
	    public Usuario LoginFacebook(string email, string facebookId, [FromBody]Object obj)
	    {
		    try
		    {
			    return Servico.LoginFacebook(email, facebookId);
		    }
		    catch (BusinessRuleException ex)
		    {
			    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) {ReasonPhrase = ex.Message});
		    }
	    }


	    [AllowAnonymous]
	    [HttpPost]
	    [Route("formularioContato")]
	    public void FormularioContato([FromBody]FormularioContato formulario)
	    {
		    Servico.EnviarFormularioContato(formulario, Resources.Contato);
		}
	}
}