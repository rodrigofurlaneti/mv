using System;
using System.Collections.Generic;
using System.Linq;
using Core.Validators;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace Dominio
{
    public interface IFornecedorServico : IBaseServico<Fornecedor>
    {
        Usuario RecuperaUsuarioPorPessoa(int pessoaId);
        void ValidaFornecedor(int pessoaId, Fornecedor fornecedor);
        void ValidaESalva(int pessoaId, Fornecedor fornecedor);
        IList<Fornecedor> BuscarFornecedors();
        void SalvarProdutoPrecoFornecedor(int fornecedorId, ProdutoPreco produtoPreco);
        Produto BuscaProdutoPorCodBarras(int lojaId, int codigo);
    }

    public class FornecedorServico : BaseServico<Fornecedor, IFornecedorRepositorio>, IFornecedorServico
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IProdutoPrecoRepositorio _produtoPrecoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IDepartamentoProdutoRepositorio _departamentoProdutoRepositorio;

        public FornecedorServico(IFornecedorRepositorio fornecedorRepositorio, IProdutoPrecoRepositorio produtoPrecoRepositorio, IProdutoRepositorio produtoRepositorio
            , IDepartamentoProdutoRepositorio departamentoProdutoRepositorio)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
            _produtoPrecoRepositorio = produtoPrecoRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _departamentoProdutoRepositorio = departamentoProdutoRepositorio;
        }

        public Usuario RecuperaUsuarioPorPessoa(int pessoaId)
        {
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            return usuarioServico.BuscarPor(x => x.Pessoa.Id == pessoaId).FirstOrDefault();
        }

        public void ValidaFornecedor(int pessoaId, Fornecedor fornecedor)
        {
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            var usuarios = usuarioServico.BuscarPor(x => x.Pessoa.Id == pessoaId);
            if (pessoaId == 0 || usuarios == null || !usuarios.Any())
                throw new Exception("Usuário não encontrado");
            
            if (fornecedor.Id <= 0 && BuscarPor(x => x.Cnpj == fornecedor.Cnpj).Count > 0)
                throw new Exception("CNPJ existente para outro estabelecimento cadastrado!");

            if (string.IsNullOrEmpty(fornecedor.RazaoSocial))
                throw new Exception("Informe a Razão Social");

        }

        public void ValidaESalva(int pessoaId, Fornecedor fornecedor)
        {
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            var enderecoServico = ServiceLocator.Current.GetInstance<IEnderecoServico>();

            ValidaFornecedor(pessoaId, fornecedor);

            if (fornecedor.Endereco != null && !string.IsNullOrEmpty(fornecedor.Endereco.Logradouro))
                enderecoServico.Salvar(fornecedor.Endereco);
            else
                fornecedor.Endereco = null;

            fornecedor.Id = SalvarComRetorno(fornecedor);
        }

        public IList<Fornecedor> BuscarFornecedors()
        {
            return _fornecedorRepositorio.BuscarFornecedors();
        }

        public void SalvarProdutoPrecoFornecedor(int fornecedorId, ProdutoPreco produtoPreco)
        {
            var fornecedor = BuscarPorId(fornecedorId);

            produtoPreco.Fornecedor = fornecedor ?? throw new Exception("Fornecedor não encontrada!");

            var produto = produtoPreco.Produto;

            foreach (var informacao in produto.Informacoes)
            {
                if (produto.Id != 0)
                {
                    var produtoAtual = _produtoRepositorio.GetById(produto.Id);

                    if (produtoAtual.Informacoes.Count(x => x.Tipo.Equals(informacao.Tipo)) > 0 && informacao.Tipo == 1)
                    {
                        informacao.Id = produtoAtual.Informacoes.FirstOrDefault(x => x.Tipo.Equals(informacao.Tipo)).Id;
                    }
                }
                informacao.Produto = produto;
            }

            var departamentoProduto = _departamentoProdutoRepositorio.GetById(produto.DepartamentoProduto.Id);

            produto.CategoriaProduto = departamentoProduto.CategoriaProduto;
            produto.DepartamentoProduto = departamentoProduto;

            _produtoRepositorio.Save(produto);

            _produtoPrecoRepositorio.Save(produtoPreco);

            if (fornecedor.FornecedorProdutos == null)
                fornecedor.FornecedorProdutos = new List<FornecedorProduto>();

            if (fornecedor.FornecedorProdutos.Count(x => x.Produto.Id == produto.Id) <= 0)
            {
                fornecedor.FornecedorProdutos.Add(new FornecedorProduto { Produto = produto });

                Salvar(fornecedor);
            }
        }

        public Produto BuscaProdutoPorCodBarras(int lojaId, int codigo)
        {
            var loja = BuscarPorId(lojaId);
            if (loja == null)
                throw new Exception($"A loja {lojaId} não foi encontrada.");

            return _produtoPrecoRepositorio.GetByFornecedorProduto(lojaId, codigo).Produto;
        }
    }
}