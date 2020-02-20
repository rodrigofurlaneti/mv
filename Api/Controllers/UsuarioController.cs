using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Api.Properties;
using Core.Exceptions;
using Core.Extensions;
using Dominio;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
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
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));

            Servico.RecuperarSenha(login, Resources.RecuperarSenha);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("account/register")]
        public IHttpActionResult Register(RegisterModel registerModel)
        {
            var pessoa = _pessoaServico.BuscarPorId(registerModel.Pessoa.Id);
            Usuario usuario = null;
            if (pessoa != null)
                usuario = Servico.BuscarPor(x => x.Pessoa.Id == pessoa.Id).FirstOrDefault();
            string senha = null;
            if (usuario != null)
                senha = usuario != null && string.IsNullOrEmpty(registerModel.Senha) && string.IsNullOrEmpty(registerModel.FacebookId) ? usuario.Senha : registerModel.Senha;
            
            if (registerModel.Id <= 0 && pessoa == null)
            {
                pessoa = registerModel.Pessoa;
                pessoa.Id = _pessoaServico.SalvarComRetorno(registerModel.Pessoa);

                Servico.Register(pessoa, senha, registerModel.Id, registerModel.FacebookId, true);
            }
            else
            {
                foreach (var contato in registerModel.Pessoa.Contatos)
                {
                    if (pessoa.Contatos.Count(x => x.Contato.Tipo == contato.Contato.Tipo) >= 1)
                    {
                        var contatoEmail = pessoa.Contatos.FirstOrDefault(x => x.Contato.Tipo == Entidade.Uteis.TipoContato.Email);
                        if (contato.Contato.Tipo == Entidade.Uteis.TipoContato.Email)
                        {
                            if (pessoa.Contatos.Count(x => x.Contato.Email == contato.Contato.Email) <= 0)
                            {
                                pessoa.Contatos.Add(contato);
                                pessoa.Contatos.Remove(contatoEmail);
                            }
                        }

                        var contatoCelular = pessoa.Contatos.FirstOrDefault(x => x.Contato.Tipo == Entidade.Uteis.TipoContato.Celular);
                        if (contato.Contato.Tipo == Entidade.Uteis.TipoContato.Celular)
                        {
                            if (pessoa.Contatos.Count(x => x.Contato.Numero == contato.Contato.Numero) <= 0)
                            {
                                pessoa.Contatos.Add(contato);
                                pessoa.Contatos.Remove(contatoCelular);
                            }
                        }
                    }
                }

                _pessoaServico.Salvar(pessoa);

                Servico.Register(pessoa, senha, registerModel.Id, registerModel.FacebookId);
            }

            return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString().Replace(Request.RequestUri.AbsolutePath, "/api/client")) + registerModel.Pessoa.Id), registerModel.Pessoa.Id);
        }

        [AllowAnonymous, HttpPost, Route("account/registerTerminal")]
        public IHttpActionResult RegisterTerminal([FromBody]RegisterTerminalModel registerModel)
        {
            var terminalCobrancaServico = ServiceLocator.Current.GetInstance<ITerminalCobrancaServico>();

            var terminal = Servico.RetornarPorUsuario(registerModel.SerialNumber);
            if (terminal == null)
            {
                Servico.RegisterTerminal(registerModel.SerialNumber, registerModel.Senha, registerModel.FacebookId);
                terminalCobrancaServico.Salvar(new TerminalCobranca
                {
                    DataInsercao = DateTime.Now,
                    NumeroSerial = registerModel.SerialNumber,
                    Modelo = registerModel.Model
                });
            }

            return Ok();
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
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("formularioContato")]
        public void FormularioContato([FromBody]FormularioContato formulario)
        {
            Servico.EnviarFormularioContato(formulario, Resources.Contato);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("account/alterpassword")]
        public IHttpActionResult AlterarSenha(RegisterModel registerModel)
        {
            Pessoa pessoa;

            if (registerModel.Pessoa.Id == 0)
            {
                pessoa = new Pessoa { Sexo = "", Nome = "" };
                pessoa.Id = _pessoaServico.SalvarComRetorno(pessoa);
            }
            else
                pessoa = _pessoaServico.BuscarPorId(registerModel.Pessoa.Id);

            var usuario = Servico.BuscarPorId(registerModel.Id);
            var senha = usuario != null && string.IsNullOrEmpty(registerModel.Senha) && string.IsNullOrEmpty(registerModel.FacebookId) ? usuario.Senha : registerModel.Senha;

            usuario.Senha = Crypt.EnCrypt(senha, ConfigurationManager.AppSettings["CryptKey"]);
            usuario.PrimeiroLogin = registerModel.PrimeiroLogin;
            usuario.IsEncrypted = true;
            usuario.Pessoa = pessoa;

            foreach (var contato in registerModel.Pessoa.Contatos)
            {
                var contatoEmail = pessoa.Contatos.FirstOrDefault(x => x.Contato.Tipo == Entidade.Uteis.TipoContato.Email);
                if (contato.Contato.Tipo == Entidade.Uteis.TipoContato.Email)
                {
                    if (pessoa.Contatos.Count(x => x.Contato.Email == contato.Contato.Email) <= 0)
                    {
                        pessoa.Contatos.Add(contato);
                        pessoa.Contatos.Remove(contatoEmail);
                    }
                }

                var contatoCelular = pessoa.Contatos.FirstOrDefault(x => x.Contato.Tipo == Entidade.Uteis.TipoContato.Celular);
                if (contato.Contato.Tipo == Entidade.Uteis.TipoContato.Celular)
                {
                    if (pessoa.Contatos.Count(x => x.Contato.Numero == contato.Contato.Numero) <= 0)
                    {
                        pessoa.Contatos.Add(contato);
                        pessoa.Contatos.Remove(contatoCelular);
                    }
                }
            }

            _pessoaServico.Salvar(pessoa);

            Servico.Salvar(usuario);

            return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString().Replace(Request.RequestUri.AbsolutePath, "/api/client")) + registerModel.Pessoa.Id), registerModel.Pessoa.Id);
        }
    }
}