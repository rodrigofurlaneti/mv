using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
{
    [RoutePrefix("api/produto")]
    public class ProdutoController : BaseController<Produto, IProdutoServico>
    {
        [HttpGet]
        [Route("produtosPorDepartamento/{departamentoId}/{lojaId}")]
        public List<Produto> GetProdutosPorDepartamento(int departamentoId, int lojaId, int inicio, int quantidade)
        {
            return Servico.GetProdutosPorDepartamento(departamentoId, lojaId).Skip(inicio).Take(quantidade).ToList();
        }

        [HttpGet]
        [Route("produtosPorCategoria/{categoriaId}/{lojaId}")]
        public List<Produto> GetProdutosPorCategoria(int categoriaId, int lojaId, int inicio, int quantidade)
        {
            return Servico.GetProdutosPorCategoria(categoriaId, lojaId).Skip(inicio).Take(quantidade).ToList();
        }

        [HttpGet]
        [Route("produtoPorCodigoBarras/{codigoBarras}")]
        public ProdutoModelView GetProdutoPorCodigoBarras(long codigoBarras)
        {
            return new ProdutoModelView(Servico.BuscarPor(x => x.CodigoBarras == codigoBarras).FirstOrDefault());
        }

        [HttpGet]
        [Route("produtos")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutos(int inicio, int quantidade)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            foreach (var preco in produtoPrecoBusiness.BuscarPor(x => x.InicioVigencia <= DateTime.Now
                                                                && x.FimVigencia >= DateTime.Now && x.Status).Skip(inicio).Take(quantidade))
            {
                produtos.Add(new ProdutoPrecoModelView(preco.Produto, preco));
            }
            return produtos;
        }

        [HttpGet]
        [Route("produtosPorNome")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutos(string nome, int inicio, int quantidade)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            foreach (var preco in produtoPrecoBusiness.BuscarPor(x => x.InicioVigencia <= DateTime.Now
                                                                && x.FimVigencia >= DateTime.Now && x.Status
                                                                && x.Produto.Nome.Contains(nome)
                                                                || x.Produto.DepartamentoProduto.Nome.Contains(nome))
                                                                .Skip(inicio).Take(quantidade))
            {
                produtos.Add(new ProdutoPrecoModelView(preco.Produto, preco));
            }
            return produtos;
        }

        [HttpGet]
        [Route("produtoPorId/{id}")]
        public ProdutoPrecoModelView GetProdutoPorId(int id)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            var preco = produtoPrecoBusiness.BuscarPor(x => x.Produto.Id == id).FirstOrDefault();

            if (preco == null)
                return null;

            return new ProdutoPrecoModelView(preco.Produto, preco);
        }

        [HttpGet]
        [Route("ofertas")]
        public IEnumerable<ProdutoPrecoModelView> GetOfertas(int inicio, int quantidade, string classificacao = "")
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            if (string.IsNullOrEmpty(classificacao))
                foreach (var preco in produtoPrecoBusiness.BuscarPor(x => x.InicioVigencia <= DateTime.Now
                                                                && x.FimVigencia >= DateTime.Now && x.Status).Skip(inicio).Take(quantidade))
                {
                    produtos.Add(new ProdutoPrecoModelView(preco.Produto, preco));
                }
            else
                foreach (var preco in produtoPrecoBusiness.BuscarPor(x => x.Loja.Classificacao == classificacao
                                                                && x.InicioVigencia <= DateTime.Now
                                                                && x.FimVigencia >= DateTime.Now && x.Status).Skip(inicio).Take(quantidade))
                {
                    produtos.Add(new ProdutoPrecoModelView(preco.Produto, preco));
                }
            return produtos;
        }

        [HttpGet]
        [Route("produtosPorCategoria/{categoriaId}")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutosPorCategoria(int categoriaId, int inicio, int quantidade)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            foreach (var preco in produtoPrecoBusiness.BuscarPor(x => x.InicioVigencia <= DateTime.Now
                                                            && x.FimVigencia >= DateTime.Now && x.Status
                                                            && x.Produto.DepartamentoProduto.CategoriaProduto.Id == categoriaId).Skip(inicio).Take(quantidade))
            {
                produtos.Add(new ProdutoPrecoModelView(preco.Produto, preco));
            }

            return produtos;
        }
    }
}