using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IProdutoPrecoAplicacao : IBaseAplicacao<ProdutoPreco>
    {
        void Salvar(ProdutoPrecoViewModel model);
        List<ProdutoPrecoViewModel> BuscarPorProdutoEFornecedor(string produtoId, string fornecedorId);
        IList<ProdutoPrecoViewModel> GetByFornecedorProduto(int fornecedorId);
        IList<ProdutoPrecoViewModel> ListProdutosVigentes();
    }

    public class ProdutoPrecoAplicacao : BaseAplicacao<ProdutoPreco, IProdutoPrecoServico>, IProdutoPrecoAplicacao
    {
        private readonly IProdutoPrecoServico _produtoPrecoServico;
        public ProdutoPrecoAplicacao(IProdutoPrecoServico produtoPrecoServico)
        {
            _produtoPrecoServico = produtoPrecoServico;
        }

        public void Salvar(ProdutoPrecoViewModel viewModel)
        {

            if (string.IsNullOrEmpty(viewModel.InicioVigencia) || string.IsNullOrEmpty(viewModel.FimVigencia)) throw new Exception("Campos de vigencias não preenchido!");

            var anoInicio = Convert.ToInt32(viewModel.InicioVigencia.Split('/')[2]);
            var mesInicio = Convert.ToInt32(viewModel.InicioVigencia.Split('/')[1]);
            var diaInicio = Convert.ToInt32(viewModel.InicioVigencia.Split('/')[0]);

            var anoFim = Convert.ToInt32(viewModel.FimVigencia.Split('/')[2]);
            var mesFim = Convert.ToInt32(viewModel.FimVigencia.Split('/')[1]);
            var diaFim = Convert.ToInt32(viewModel.FimVigencia.Split('/')[0]);

            var dataInicio = new DateTime(anoInicio, mesInicio, diaInicio);
            var dataFim = new DateTime(anoFim, mesFim, diaFim);

            if (dataInicio >= dataFim) throw new Exception("Data Fim deve ser maior que data inicio!");
            if (Convert.ToInt32(viewModel.ProdutoId) == 0) throw new Exception("Campo Produto não preenchido!");
            if (Convert.ToInt32(viewModel.FornecedorId) == 0) throw new Exception("Campo Fornecedor não preenchido!");
            if (Convert.ToInt32(viewModel.Loja.Id) == 0) throw new Exception("Campo Loja não preenchido!");
            if (Convert.ToDecimal(viewModel.Valor.Replace("R$", string.Empty)) == 0) throw new Exception("Campo Valor não preenchido!");


            Salvar(new ProdutoPreco
            {
                Id = viewModel.Id,
                Produto = new Produto { Id = Convert.ToInt32(viewModel.ProdutoId) },
                Fornecedor = new Fornecedor { Id = Convert.ToInt32(viewModel.FornecedorId) },
                Loja = new Loja { Id = Convert.ToInt32(viewModel.Loja.Id) },
                Valor = Convert.ToDecimal(viewModel.Valor.Replace("R$ ", "")),
                ValorDesconto = Convert.ToDecimal(viewModel.ValorDesconto.Replace("R$ ", "")),
                InicioVigencia = dataInicio,
                FimVigencia = dataFim
            });
        }

        public List<ProdutoPrecoViewModel> BuscarPorProdutoEFornecedor(string produtoId, string fornecedorId)
        {
            return BuscarPor(x =>
                    x.Produto.Id == Convert.ToInt32(produtoId) && x.Fornecedor.Id == Convert.ToInt32(fornecedorId))
                .Select(x => new ProdutoPrecoViewModel(x)).ToList();
        }

        public IList<ProdutoPrecoViewModel> GetByFornecedorProduto(int fornecedorId)
        {
            return _produtoPrecoServico.GetByFornecedorProduto(fornecedorId).Select(x => new ProdutoPrecoViewModel(x)).ToList();
        }

        public IList<ProdutoPrecoViewModel> ListProdutosVigentes()
        {
            return _produtoPrecoServico.ListProdutosVigentes().Select(x => new ProdutoPrecoViewModel(x)).ToList();
        }
    }
}