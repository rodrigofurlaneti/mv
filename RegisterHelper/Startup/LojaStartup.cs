using System;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Aplicacao;
using Entidade;
using Entidade.Uteis;

namespace InitializerHelper.Startup
{
    public static class LojaStartup
    {
        #region Private Members
        private static void AdicionaLoja()
        {
            var lojaAplicacao = ServiceLocator.Current.GetInstance<ILojaAplicacao>();
            var loja = lojaAplicacao.PrimeiroPor(x => x.Id.Equals("123456789"));
            if (loja != null)
                return;
            
            //adicionar produtos
            var produtoAplicacao = ServiceLocator.Current.GetInstance<IProdutoAplicacao>();
            var p1 = new Produto
            {
                Codigo = "789456123",
                Nome = "Arroz tião josé",
                Descricao = "Arroz tipo 1"
            };
            var p2 = new Produto
            {
                Codigo = "789456124",
                Nome = "Feijão tião josé",
                Descricao = "Feijão carioquinha"
            };
            var produtos = new List<Produto> { p1, p2 };
            produtos.ForEach(p => { produtoAplicacao.Salvar(p); });
            // informacao produto
            var informacaoProdutoAplicacao = ServiceLocator.Current.GetInstance<IInformacaoProdutoAplicacao>();
            var ip1 = new InformacaoProduto { Descricao = "Arroz branco polído", Tipo = (int)TipoInfoProduto.Detalhe, Produto = p1 };
            var ip2 = new InformacaoProduto { Descricao = "Feijão marrom bruto", Tipo = (int)TipoInfoProduto.Detalhe, Produto = p2 };
            var ips = new List<InformacaoProduto> { ip1, ip2 };
            ips.ForEach(i => { informacaoProdutoAplicacao.Salvar(i); }); 
            //adicionar loja
            loja = new Loja
            {
                Descricao = "Supermercado Amigão",
                Endereco = GetEndereco(),
                Telefone = "1121234455",
                //Produtos = produtos
            };
            lojaAplicacao.Salvar(loja);
            //preço produto
            var produtoPrecoAplicacao = ServiceLocator.Current.GetInstance<IProdutoPrecoAplicacao>();
            var count = 1;
            produtos.ForEach(p =>
            {
                produtoPrecoAplicacao.Salvar(new ProdutoPreco { Valor = 10.90M + count, Loja = loja, Produto = p });
                count++;
            });
        }

        private static Endereco GetEndereco()
        {
           //endereco
            var enderecoAplicacao = ServiceLocator.Current.GetInstance<IEnderecoAplicacao>();
            var cidadeAplicacao = ServiceLocator.Current.GetInstance<ICidadeAplicacao>();
            var cidade = cidadeAplicacao.PrimeiroPor(x => x.Descricao.Equals("São Paulo"));
            var endereco = new Endereco
            {
                Cep = "04742-001",
                Bairro = "Santo Amaro",
                Logradouro = "R. Padre José de Anchieta",
                Numero = "953",
                Tipo = 2,
                Complemento = "Loja 1",
                Descricao = "Loja",
                Latitude = "-23.6508040",
                Longitude = "-46.7002650",
                Cidade = cidade
            };
            enderecoAplicacao.Salvar(endereco);
            return endereco;
        }
        #endregion

        #region Public Members

        public static void Start()
        {
            //AdicionaLoja();
        }

        #endregion
    }
}