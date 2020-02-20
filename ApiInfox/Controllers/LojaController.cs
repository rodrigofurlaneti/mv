using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using ApiInfox.Base;
using ApiInfox.Models;
using Core.Exceptions;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    //[EnableCors(origins: "http:/localhost:8100", headers: "*", methods: "*")]
    [RoutePrefix("api/loja")]
    public class LojaController : BaseController<Loja, ILojaServico>
    {
        [HttpGet]
        [Route("lojasProximas")]
        public IEnumerable<LojaModelView> GetLojas(float latitude, float longitude)
        {
            var lojas = new List<LojaModelView>();
            var lojasBase = Servico.BuscarLojas();

            foreach (var loja in lojasBase)
                lojas.Add(new LojaModelView(loja, latitude, longitude));

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia);
        }

        [HttpGet]
        [Route("{id}/produtos")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutos(int id, int inicio, int quantidade)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            foreach (var preco in produtoPrecoBusiness.BuscarPor(x => x.Loja.Id == id).Skip(inicio).Take(quantidade))
            {
                produtos.Add(new ProdutoPrecoModelView(preco.Produto, preco));
            }
            //
            return produtos;
        }

        [HttpGet]
        [Route("{id}/produtoCodBarras")]
        public ProdutoPrecoModelView GetProdutoPorCodigoBarras(int id, string codBarras)
        {
            var produto = Servico.BuscaProdutoPorCodBarras(id, codBarras);
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            if (produto == null)
                return null;

            var preco = produtoPrecoBusiness.GetByLojaProduto(id, produto.Id);
            return new ProdutoPrecoModelView(produto, preco);
        }

        [HttpGet]
        [Route("lojasUsuario")]
        public IEnumerable<LojaModelView> GetLojasUsuario(int pessoa, float latitude, float longitude)
        {
            var usr = Servico.RecuperaUsuarioPorPessoa(pessoa);
            var lojas = usr.ListaUsuarioLoja.Select(item => new LojaModelView(item.Loja, latitude, longitude)).ToList();
            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia);
        }

        [HttpGet]
        [Route("tiposClassificacao")]
        public IEnumerable<ChaveValorModelView> GetTiposClassificacao()
        {
            return Enum.GetValues(typeof(TipoClassificacao)).Cast<TipoClassificacao>().Select(x => new ChaveValorModelView { Id = (int)x, Descricao = x.ToDescription() });
        }

        [HttpGet]
        [Route("classificacoes/{tipo}")]
        public IEnumerable<ChaveValorModelView> GetClassificacoes(int tipo)
        {
            return tipo != (int)TipoClassificacao.Tradicional
                ? new List<ChaveValorModelView>()
                : Enum.GetValues(typeof(ClassificacaoLoja)).Cast<ClassificacaoLoja>().Select(x => new ChaveValorModelView { Id = (int)x, Descricao = x.ToDescription() });
        }

        [HttpPost]
        [Route("salvarLoja/{id}")]
        public virtual void SalvarLoja(int id, [FromBody] Loja loja)
        {
            try
            {
                Servico.ValidaESalva(id, loja);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        
        [HttpPost]
        [Route("salvarProdutoPrecoLoja/{id}")]
        public virtual void SalvarProdutoPrecoLoja(int id, [FromBody] ProdutoPreco produtoPreco)
        {
            try
            {
                Servico.SalvarProdutoPrecoLoja(id, produtoPreco);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}