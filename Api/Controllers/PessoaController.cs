using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Api.Base;
using Core.Exceptions;
using System.Net;
using System.Web.UI.WebControls;
using Api.Models;
using Dominio;
using Entidade;
using System;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/pessoa")]
    public class PessoaController : BaseController<Pessoa, IPessoaServico>
    {
        private readonly IEnderecoServico _enderecoServico;
        private readonly IDocumentoServico _documentoServico;
        private readonly IContatoServico _contatoServico;
        private readonly ICartaoServico _cartaoServico;

        public PessoaController(IEnderecoServico enderecoServico, IDocumentoServico documentoServico, IContatoServico contatoServico, ICartaoServico cartaoServico)
        {
            _enderecoServico = enderecoServico;
            _documentoServico = documentoServico;
            _contatoServico = contatoServico;
            _cartaoServico = cartaoServico;
        }

        [HttpGet]
        [Route("{id}")]
        public override Pessoa Get(int id)
        {
            try
            {
                var pessoa = Servico.BuscarPorId(id);

                if (pessoa?.Cartoes != null && pessoa.Cartoes.Any())
                    pessoa.Cartoes = _cartaoServico.DescriptografarCartoes(pessoa.Cartoes);

                return pessoa;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("BuscarPorCPF/{cpf}")]
        public PessoaModelView GetByCPF(string cpf)
        {
            try
            {
                var pessoa = Servico.BuscarPor(x => x.Documentos.Count(y => y.Numero.Replace(".", "").Replace("-", "").Equals(cpf)) > 0).LastOrDefault();

                return new PessoaModelView(pessoa);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }


        [HttpGet]
        [Route("ObterPorCartao/{numero}")]
        public Pessoa ObterPorCartao(string numero)
        {
            return Servico.ObterPorCartao(numero);
        }


        [HttpPost]
        [Route("{id}/adicionaritem")]
        public Pessoa AdicionarItem(int id, Endereco item)
        {
            try
            {
                var pessoa = Servico.BuscarPorId(id);

                pessoa.EnderecosEntrega.Add(item);

                return pessoa;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("descontosexclusivos/{id}")]
        public IList<DescontoPessoaModelView> GetDescontosPessoa(int id)
        {
            try
            {
                var pessoa = Servico.BuscarPorId(id);
                return pessoa.ListaDescontoPessoa.Select(item => new DescontoPessoaModelView(item.Desconto, pessoa, item.ProdutoPreco)).ToList();
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("descontos/{id}")]
        public IList<DescontoPessoaModelView> GetDescontos(int id)
        {
            try
            {
                var pessoa = Servico.BuscarPorId(id);

                return pessoa?.ListaDescontoPessoa?
                        .Where(item => item.ProdutoPreco != null)?
                            .Select(item => new DescontoPessoaModelView(item.Desconto, pessoa, item.ProdutoPreco))?
                            .OrderByDescending(x => x.ProdutoPreco.Produto.Codigo)?
                            .ToList() ?? new List<DescontoPessoaModelView>();
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}