using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using Core.Exceptions;
using Dominio;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace ApiInfox.Controllers
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
                //var produtosPreco = produtoPrecoBusiness.GetByFornecedorProduto(fornecedor.Id);
                //foreach (var produtoPreco in produtosPreco)
                //{
                //    if (fornecedorModelView.Produtos == null)
                //        fornecedorModelView.Produtos = new List<ProdutoPrecoModelView>();
                    
                //    if (produtoPreco.InicioVigencia <= DateTime.Now && produtoPreco.FimVigencia >= DateTime.Now)
                //        fornecedorModelView.Produtos.Add(new ProdutoPrecoModelView(produtoPreco.Produto, produtoPreco));
                //}
                //if ((fornecedorModelView?.Produtos?.Any() ?? false))
                    fornecedores.Add(fornecedorModelView);
            }

            return fornecedores;
        }

        [HttpGet]
        [Route("fornecedoresProximos")]
        public IEnumerable<FornecedorModelView> GetFornecedores(float latitude, float longitude)
        {
            var lojas = new List<FornecedorModelView>();
            var lojasBase = Servico.Buscar();

            foreach (var loja in lojasBase)
                lojas.Add(new FornecedorModelView(loja, latitude, longitude));

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia);
        }

        [HttpGet]
        [Route("produtos/{id}")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutos(int id, int lojaId, int inicio, int quantidade)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();

            if (Servico.BuscarPorId(id).FornecedorProdutos == null) return new List<ProdutoPrecoModelView>();

            foreach (var produto in Servico.BuscarPorId(id).FornecedorProdutos.Select(x => x.Produto).Where(p => p.Lojas.Any(l => l.Id == lojaId)).Skip(inicio).Take(quantidade))
            {
                var preco = produtoPrecoBusiness.GetByFornecedorProduto(id, produto.Id);
                if (preco == null)
                    continue;

                produtos.Add(new ProdutoPrecoModelView(produto, preco));
            }
            
            return produtos;
        }

        [HttpGet]
        [Route("produtos/{id}/{usuarioId}")]
        public IEnumerable<ProdutoPrecoModelView> GetProdutos(int id, int usuarioId)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();
            var produtos = new List<ProdutoPrecoModelView>();
            var usuarioBussiness = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            var usuario = usuarioBussiness.BuscarPorId(usuarioId);
            var descontos = usuario.Pessoa.ListaDescontoPessoa;

            foreach (var produto in Servico.BuscarPorId(id).FornecedorProdutos.Select(x => x.Produto))
            {
                var preco = produtoPrecoBusiness.GetByFornecedorProduto(id, produto.Id);
                if (preco == null)
                    continue;

                if (descontos != null && descontos.Any(x => x.ProdutoPreco.Id == preco.Id))
                    preco.Valor = descontos?.FirstOrDefault(x => x.ProdutoPreco.Id == preco.Id)?.ValorProdutoComDesconto ?? preco.Valor;

                produtos.Add(new ProdutoPrecoModelView(produto, preco));
            }
            
            return produtos;
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
    }
}