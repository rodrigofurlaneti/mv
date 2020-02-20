using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace Dominio
{
    public interface IListaCompraServico : IBaseServico<ListaCompra>
    {
        ListaCompra BuscaListaAtiva(int usuarioId);
        ListaCompra AtualizaLista(int id);
        ListaCompra AdicionaItem(int id, ItemCompra item);
        ListaCompra AlteraItem(int id, ItemCompra item);
        ListaCompra RemoveItem(int id, ItemCompra item);
        ListaCompra RemoverTodosItens(int id);
        ListaCompra AtribuiDescontoAListaCompra(ListaCompra listaCompra);
        ListaCompra MudarLojaAtiva(int listaAtual, int novaLoja);
        ListaCompra RecuperarItens(int listaAtual, int listaAntiga);
        ListaCompra BuscaListaAtivaPorLoja(int usuarioId, int lojaId);
    }

    public class ListaCompraServico : BaseServico<ListaCompra, IListaCompraRepositorio>, IListaCompraServico
    {
        private readonly IListaCompraRepositorio _listaCompraRepositorio;
        private readonly ICupomServico _cupomServico;
        private readonly IProdutoPrecoServico _produtoPrecoServico;
        private readonly IItemCompraServico _itemCompraServico;
        private readonly ILojaRepositorio _lojaRepositorio;

        public ListaCompraServico(IListaCompraRepositorio listaCompraRepositorio,
                                    ICupomServico cupomServico,
                                    IProdutoPrecoServico produtoPrecoServico,
                                    IItemCompraServico itemCompraServico,
                                    ILojaRepositorio lojaRepositorio)
        {
            _listaCompraRepositorio = listaCompraRepositorio;
            _cupomServico = cupomServico;
            _produtoPrecoServico = produtoPrecoServico;
            _itemCompraServico = itemCompraServico;
            _lojaRepositorio = lojaRepositorio;
        }

        public ListaCompra BuscaListaAtiva(int usuarioId)
        {
            // Efetua clear para obter os itens recém-gravados
            Repositorio.Clear();

            var ultimaLista = BuscarPor(x => x.Usuario.Id == usuarioId)?.OrderByDescending(x => x.DataInsercao)?.FirstOrDefault();
            if (ultimaLista == null)
                return null;

            var pedidoServico = ServiceLocator.Current.GetInstance<IPedidoServico>();
            var listaPedidoPesquisado = pedidoServico.BuscarPor(x => x.ListaCompra.Id == ultimaLista.Id)?.OrderByDescending(x => x.DataInsercao)?.ToList();
            if (listaPedidoPesquisado == null || !(listaPedidoPesquisado?.LastOrDefault()?.ListaHistorico.Any(x => x.StatusPedido == StatusPedido.AguardandoPagamento) ?? false))
            {
                return AtribuiDescontoAListaCompra(ultimaLista);
            }

            return null;
        }

        public ListaCompra BuscaListaAtivaPorLoja(int usuarioId, int lojaId)
        {
            // Efetua clear para obter os itens recém-gravados
            Repositorio.Clear();

            var ultimaLista = BuscarPor(x => x.Usuario.Id == usuarioId && x.Loja.Id == lojaId)?.OrderByDescending(x => x.DataInsercao)?.FirstOrDefault();
            if (ultimaLista == null)
                return null;

            var pedidoServico = ServiceLocator.Current.GetInstance<IPedidoServico>();
            var listaPedidoPesquisado = pedidoServico.BuscarPor(x => x.ListaCompra.Id == ultimaLista.Id)?.OrderByDescending(x => x.DataInsercao)?.ToList();
            if (listaPedidoPesquisado == null || !(listaPedidoPesquisado?.LastOrDefault()?.ListaHistorico.Any(x => x.StatusPedido == StatusPedido.AguardandoPagamento) ?? false))
            {
                return AtribuiDescontoAListaCompra(ultimaLista);
            }

            return new ListaCompra { Usuario = new Usuario { Id = usuarioId }, Loja = new Loja { Id = lojaId } };
        }

        public ListaCompra AtualizaLista(int id)
        {
            // Efetua clear para obter os itens recém-gravados
            Repositorio.Clear();

            // Obtém lista atualizada
            var listaCompra = BuscarPorId(id);
            if (listaCompra == null)
                throw new Exception("Lista de compras não encontrada.");

            listaCompra.Total = listaCompra.Itens.Sum(x => x.Total);

            //Recalcular Desconto do Cupom
            listaCompra.ValorCupom = _cupomServico.CalcularValorCupom(listaCompra.Total, listaCompra.Cupom);

            Salvar(listaCompra);

            return AtribuiDescontoAListaCompra(listaCompra);
        }

        public ListaCompra AdicionaItem(int id, ItemCompra item)
        {
            var listaCompra = BuscarPorId(id);
            if (listaCompra == null)
                throw new Exception("Lista de compras não encontrada.");

            var descontos = listaCompra?.Usuario?.Pessoa?.ListaDescontoPessoa ?? new List<DescontoPessoa>();

            // Trata casos em que o cliente volta à lista de produtos e pede para adicionar um produto já existente no carrinho
            var itemCompras = ServiceLocator.Current.GetInstance<ItemCompraServico>();

            if (item.PlanoVenda == null)
            {
                var itemExistente = listaCompra.Itens.FirstOrDefault(x => x.Produto.Id.Equals(item.Produto.Id) && x.Produto.DepartamentoProduto.CategoriaProduto.Id != (int)CategoriaDeProduto.Convenio);
                if (itemExistente == null)
                {
                    if (descontos != null && descontos.Count > 0)
                        item.Total = item.Quantidade * (descontos?.FirstOrDefault(x => (x?.ProdutoPreco?.Id ?? 0) == item.Preco.Id)?.ValorProdutoComDesconto ?? item.Preco.Valor);
                    else
                        item.Total = item.Quantidade * item.Preco.Valor;
                    item.ListaCompra = listaCompra;
                    itemCompras.Salvar(item);
                }
                else
                {
                    itemExistente.Quantidade += item.Quantidade;
                    itemExistente.Total = itemExistente.Quantidade * (descontos?.FirstOrDefault(x => (x?.ProdutoPreco?.Id ?? 0) == item.Preco.Id)?.ValorProdutoComDesconto ?? item.Preco.Valor);
                    itemCompras.Salvar(itemExistente);
                }
            }
            else
            {
                var itemExistente = listaCompra.Itens.FirstOrDefault(x => x.PlanoVenda.Id.Equals(item.PlanoVenda.Id) && x.Beneficiario.Id.Equals(item.Beneficiario.Id));

                if (itemExistente != null)
                    throw new Exception("Item ja adicionado na lista!");

                item.Total = item.Quantidade * (item.PlanoVenda.ValorDesconto != 0 ? item.PlanoVenda.ValorDesconto: item.PlanoVenda.Valor); 
                item.ListaCompra = listaCompra;
                itemCompras.Salvar(item);
            }

            return AtualizaLista(id);
        }

        public ListaCompra AlteraItem(int id, ItemCompra item)
        {
            var listaCompra = item.ListaCompra.Usuario == null ? BuscarPorId(item.ListaCompra.Id) : item.ListaCompra;
            var descontos = listaCompra?.Usuario?.Pessoa?.ListaDescontoPessoa;
            // Item já possui id e referências
            var itemCompras = ServiceLocator.Current.GetInstance<ItemCompraServico>();
            item.Total = item.Quantidade * (descontos?.FirstOrDefault(x => (x?.ProdutoPreco?.Id ?? 0) == item.Preco.Id)?.ValorProdutoComDesconto ?? item.Preco.Valor);
            itemCompras.Salvar(item);
            //
            return AtualizaLista(id);
        }

        public ListaCompra RemoveItem(int id, ItemCompra item)
        {
            var itemCompras = ServiceLocator.Current.GetInstance<ItemCompraServico>();
            itemCompras.ExcluirPorId(item.Id);
            //
            return AtualizaLista(id);
        }

        public ListaCompra RemoverTodosItens(int id)
        {
            foreach (var item in _itemCompraServico.BuscarPor(i => i.ListaCompra.Id == id))
            {
                _itemCompraServico.Excluir(item);
            }

            return AtualizaLista(id);
        }


        public ListaCompra AtribuiDescontoAListaCompra(ListaCompra listaCompra)
        {
            //Substitui Valor do Preço e atribui o desconto ao preço para exibição
            var descontos = listaCompra?.Usuario?.Pessoa?.ListaDescontoPessoa;
            if (descontos == null) return listaCompra;
            foreach (var itemCompra in listaCompra.Itens.Where(x => (x.Preco?.Id ?? 0) > 0 && descontos.Any(y => y.ProdutoPreco?.Id == x.Preco?.Id))?.ToList())
            {
                itemCompra.Preco.ValorDesconto =
                    descontos?.FirstOrDefault(x => (x?.ProdutoPreco?.Id ?? 0) == itemCompra.Preco.Id)?.ValorDesconto ??
                    0;
            }
            return listaCompra;
        }

        public ListaCompra MudarLojaAtiva(int listaAtual, int novaLoja)
        {
            var lista = BuscarPorId(listaAtual);

            var loja = _lojaRepositorio.FirstBy(x => x.Id == novaLoja);
            if (loja?.Endereco != null)
                loja.Endereco.Pessoa = null;

            lista.Loja = new Loja
            {
                Id = loja?.Id ?? 0,
                Descricao = loja?.Descricao,
                RazaoSocial = loja.RazaoSocial,
                Status = loja.Status,
                Telefone = loja.Telefone,
                Cnpj = loja.Cnpj,
                InscricaoEstadual = loja.InscricaoEstadual,
                NotaAvaliacao = loja.NotaAvaliacao,
                Endereco = loja.Endereco
            };

            VerificarVigenciaPrecos(lista.Itens, novaLoja);

            Salvar(lista);

            return lista;
        }

        public ListaCompra RecuperarItens(int listaAtual, int listaAntiga)
        {
            var atual = BuscarPorId(listaAtual);
            var antiga = BuscarPorId(listaAntiga);

            VerificarVigenciaPrecos(antiga.Itens, atual.Loja.Id);

            atual.Itens = antiga.Itens.ToList();

            Salvar(atual);

            return atual;
        }

        private void VerificarVigenciaPrecos(IEnumerable<ItemCompra> itens, int lojaId)
        {
            foreach (var item in itens)
            {
                var produtoPreco = _produtoPrecoServico.PrimeiroPor(pp => pp.Valor > 0 &&
                                                                          pp.InicioVigencia <= DateTime.Now && pp.FimVigencia >= DateTime.Now &&
                                                                          pp.Produto.Id == item.Produto.Id &&
                                                                          pp.Loja.Id == lojaId);

                item.Preco = produtoPreco;
                if (produtoPreco == null)
                {
                    item.StatusProdutoPreco = StatusProdutoPreco.Inexistente;
                }
            }
        }
    }
}