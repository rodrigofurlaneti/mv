using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.ServiceLocation;

namespace Dominio
{
    public interface ILojaServico : IBaseServico<Loja>
    {
        Produto BuscaProdutoPorCodBarras(int lojaId, string codBarras);
        Usuario RecuperaUsuarioPorPessoa(int pessoaId);
        void ValidaLoja(int pessoaId, Loja loja);
        void ValidaESalva(int pessoaId, Loja loja);
        IList<Loja> BuscarLojas(string classificacao = "");
        void SalvarProdutoPrecoLoja(int lojaId, ProdutoPreco produtoPreco);
        void SalvarLojaIndicacao(Loja loja);
        IList<HorarioFuncionamentoLoja> BuscarHorarioFuncionamento(int lojaId);
        IList<Loja> BuscaLojasPor(int estado, int cidade, string bairro, string dadosPesquisa = "");
        IList<Loja> BuscarLojasPorNome(string nome = "");
        IList<Loja> BuscarLojasPorTipoPorNome(string nomeTipo = "");
    }

    public class LojaServico : BaseServico<Loja, ILojaRepositorio>, ILojaServico
    {
        private readonly ILojaRepositorio _lojaRepositorio;
        private readonly IProdutoPrecoRepositorio _produtoPrecoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IDepartamentoProdutoRepositorio _departamentoProdutoRepositorio;
        private readonly IHorarioFuncionamentoLojaRepositorio _horarioFuncionamentoLojaRepositorio;

        public LojaServico(ILojaRepositorio lojaRepositorio, IProdutoPrecoRepositorio produtoPrecoRepositorio, IProdutoRepositorio produtoRepositorio
            , IDepartamentoProdutoRepositorio departamentoProdutoRepositorio, IHorarioFuncionamentoLojaRepositorio horarioFuncionamentoLojaRepositorio)
        {
            _lojaRepositorio = lojaRepositorio;
            _produtoPrecoRepositorio = produtoPrecoRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _departamentoProdutoRepositorio = departamentoProdutoRepositorio;
            _horarioFuncionamentoLojaRepositorio = horarioFuncionamentoLojaRepositorio;
        }

        public Produto BuscaProdutoPorCodBarras(int lojaId, string codBarras)
        {
            var loja = BuscarPorId(lojaId);
            if (loja == null)
                throw new Exception($"A loja {lojaId} não foi encontrada.");

            return _produtoPrecoRepositorio.GetByLojaProdutoCodigoDeBarras(lojaId, codBarras).Produto;
        }

        public Usuario RecuperaUsuarioPorPessoa(int pessoaId)
        {
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            return usuarioServico.BuscarPor(x => x.Pessoa.Id == pessoaId).FirstOrDefault();
        }

        public void ValidaLoja(int pessoaId, Loja loja)
        {
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            var usuarios = usuarioServico.BuscarPor(x => x.Pessoa.Id == pessoaId);
            if (pessoaId == 0 || usuarios == null || !usuarios.Any())
                throw new Exception("Usuário não encontrado");

            if (loja.Id <= 0 && BuscarPor(x => x.Cnpj == loja.Cnpj).Count > 0)
                throw new Exception("CNPJ existente para outro estabelecimento cadastrado!");

            if (string.IsNullOrEmpty(loja.RazaoSocial))
                throw new Exception("Informe a Razão Social");

        }

        public void ValidaESalva(int pessoaId, Loja loja)
        {
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            var enderecoServico = ServiceLocator.Current.GetInstance<IEnderecoServico>();
            var terminaisCobrancaServico = ServiceLocator.Current.GetInstance<ITerminalCobrancaLojaServico>();

            ValidaLoja(pessoaId, loja);

            if (loja.Endereco != null && !string.IsNullOrEmpty(loja.Endereco.Logradouro))
                enderecoServico.Salvar(loja.Endereco);
            else
                loja.Endereco = null;

            loja.Status = true;
            loja.LojaAprovada = true;

            var terminais = terminaisCobrancaServico.BuscarPor(x => x.Loja.Id == loja.Id);
            foreach (var terminal in terminais)
            {
                if (!loja.TerminaisLoja.Any(x => x.Id == terminal.Id))
                    terminaisCobrancaServico.ExcluirPorId(terminal.Id);
            }

            loja.Id = SalvarComRetorno(loja);

            var usuario = usuarioServico.BuscarPor(x => x.Pessoa.Id == pessoaId).FirstOrDefault();

            if (usuario.ListaUsuarioLoja == null)
                usuario.ListaUsuarioLoja = new List<UsuarioLoja>();

            if (usuario.ListaUsuarioLoja.Count(x => x.Loja.Id == loja.Id) <= 0)
                usuario.ListaUsuarioLoja.Add(new UsuarioLoja { Loja = loja });

            usuarioServico.Salvar(usuario);

        }

        public void SalvarLojaIndicacao(Loja loja)
        {
            var enderecoServico = ServiceLocator.Current.GetInstance<IEnderecoServico>();

            if (loja.Endereco != null && !string.IsNullOrEmpty(loja.Endereco.Logradouro))
                enderecoServico.Salvar(loja.Endereco);
            else
                loja.Endereco = null;

            loja.Status = true;
            loja.LojaAprovada = false;

            loja.Id = SalvarComRetorno(loja);

            var from = ConfigurationManager.AppSettings["EMAIL_FROM"];
            var to = ConfigurationManager.AppSettings["EMAIL_INDICACAO"];
            StringBuilder corpo = new StringBuilder();
            corpo.Append("Estabelecimento: ");
            corpo.Append(loja.Descricao);
            corpo.Append(" Telefone: ");
            corpo.Append(loja.Telefone);
            if (loja.Celular != null)
            {
                corpo.Append(" Celular: ");
                corpo.Append(loja.Celular);
            }
            if (loja.Endereco != null)
            {
                corpo.Append(" Estado: ");
                corpo.Append(loja.Endereco.Cidade.Estado.Sigla);
                corpo.Append(" Cidade: ");
                corpo.Append(loja.Endereco.Cidade.Descricao);
            }

            Mail.SendMail(to, "[MeuVale APP] - Indicação Credenciado", corpo.ToString(), from);
        }

        public IList<Loja> BuscarLojas(string classificacao = "")
        {
            var lojas = _lojaRepositorio.BuscarLojas(classificacao);

            return lojas;
        }

        public IList<Loja> BuscarLojasPorNome(string nome = "")
        {
            var lojas = _lojaRepositorio.BuscarLojasPorNome(nome);

            return lojas;
        }

        public IList<Loja> BuscarLojasPorTipoPorNome(string nomeTipo = "")
        {
            var lojas = _lojaRepositorio.BuscarLojasPorTipoPorNome(nomeTipo);

            return lojas;
        }

        public IList<HorarioFuncionamentoLoja> BuscarHorarioFuncionamento(int lojaId)
        {
            return _horarioFuncionamentoLojaRepositorio.ListBy(x => x.Loja.Id == lojaId);
        }

        public void SalvarProdutoPrecoLoja(int lojaId, ProdutoPreco produtoPreco)
        {
            var loja = BuscarPorId(lojaId);

            produtoPreco.Loja = loja ?? throw new Exception("Loja não encontrada!");

            var produto = produtoPreco.Produto;

            foreach (var informacao in produto.Informacoes)
            {
                if (produto.Id != 0)
                {
                    var produtoAtual = _produtoRepositorio.GetById(produto.Id);

                    if (produtoAtual.Informacoes.Count(x => x.Tipo.Equals(informacao.Tipo) && x.Tipo == 1) > 0)
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

            if (loja.LojaProdutos == null)
                loja.LojaProdutos = new List<LojaProduto>();

            if (loja.LojaProdutos.Count(x => x.Produto.Id == produto.Id) <= 0)
            {
                loja.LojaProdutos.Add(new LojaProduto { Produto = produto });

                Salvar(loja);
            }
        }

        public IList<Loja> BuscaLojasPor(int estado, int cidade, string bairro, string dadosPesquisa = "")
        {
            var lojas = _lojaRepositorio.BuscaLojasPor(estado, cidade, bairro, dadosPesquisa).DistinctBy(x => x.Id).ToList();

            return lojas;
        }
    }
}