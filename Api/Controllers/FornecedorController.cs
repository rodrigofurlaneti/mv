using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Core.Exceptions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/fornecedor")]
    public class FornecedorController : BaseController<Fornecedor, IFornecedorServico>
    {
        [HttpGet]
        [Route("fornecedores")]
        public IEnumerable<FornecedorModelView> GetFornecedores()
        {
            var fornecedores = new List<FornecedorModelView>();
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();

            foreach (var fornecedor in Servico.Buscar())
            {
                var fornecedorModelView = new FornecedorModelView(fornecedor);
                    fornecedores.Add(fornecedorModelView);
            }

            return fornecedores.OrderBy(x => x.Descricao).ToList();
        }

        [HttpGet]
        [Route("fornecedoresProximos")]
        public IEnumerable<FornecedorModelView> GetFornecedores(float latitude, float longitude, int inicio, int quantidade)
        {
            var lojas = new List<FornecedorModelView>();
            var lojasBase = Servico.Buscar();

            foreach (var loja in lojasBase)
                lojas.Add(new FornecedorModelView(loja, latitude, longitude));

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia).Skip(inicio).Take(quantidade);
        }

        [HttpGet]
        [Route("fornecedoresProximosPorClassificacao")]
        public IEnumerable<FornecedorModelView> GetFornecedoresPorClassificacao(float latitude, float longitude, int inicio, int quantidade, string classificacao)
        {
            var lojas = new List<FornecedorModelView>();
            var lojasBase = Servico.BuscarPor(x => x.Classificacao == classificacao || x.Classificacao == TipoClassificacao.Ambos.ToString());

            foreach (var loja in lojasBase)
                lojas.Add(new FornecedorModelView(loja, latitude, longitude));

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia).Skip(inicio).Take(quantidade);
        }

        [HttpGet]
        [Route("{id}/produtos")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutos(int id, int inicio, int quantidade)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            foreach (var preco in produtoPrecoBusiness.BuscarPor(x => x.Fornecedor.Id == id).Skip(inicio).Take(quantidade))
            {
                produtos.Add(new ProdutoPrecoModelView(preco.Produto, preco));
            }
            //
            return produtos;
        }

        [HttpGet]
        [Route("produtos/{id}/")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutos(int id)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            var usuarioBussiness = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            
            foreach (var produto in Servico.BuscarPorId(id).FornecedorProdutos.Select(x => x.Produto))
            {
                var preco = produtoPrecoBusiness.GetByFornecedorProduto(id, produto.Id);
                if (preco == null)
                    continue;
                
                produtos.Add(new ProdutoPrecoModelView(produto, preco));
            }
            
            return produtos;
        }

        [HttpGet]
        [Route("{id}/produtoCodigo")]
        public ProdutoPrecoModelView GetProdutoPorCodigoBarras(int id, int codigo)
        {
            var produto = Servico.BuscaProdutoPorCodBarras(id, codigo);
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            if (produto == null)
                return null;

            var preco = produtoPrecoBusiness.GetByLojaProduto(id, produto.Id);
            return new ProdutoPrecoModelView(produto, preco);
        }

        [HttpPost]
        [Route("salvarFornecedor/{id}")]
        public virtual void SalvarFornecedor(int id, [FromBody] Fornecedor entity)
        {
            try
            {
                Servico.ValidaESalva(id, entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("salvarProdutoPrecoFornecedor/{id}")]
        public virtual void SalvarProdutoPrecoLoja(int id, [FromBody] ProdutoPreco produtoPreco)
        {
            try
            {
                Servico.SalvarProdutoPrecoFornecedor(id, produtoPreco);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}