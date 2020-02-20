using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade;
using Entidade.Uteis;

namespace ApiInfox.Models
{
    public class ProdutoModelView
    {
        public ProdutoModelView(Produto produto)
        {
            if (produto == null)
                throw new Exception("O produto não foi encontrado.");

            Id = produto.Id;
            Nome = produto.Nome;
            Descricao = produto.Descricao;
            Codigo = produto.Codigo;

            Imagens = new List<InformacaoProduto>();
            
            Imagens = produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Imagem).ToList();
            Descricoes = produto.Informacoes.Where(x => x.Tipo == (int)TipoInfoProduto.Outros).ToList();

            Quantidade = 1;

            CodigoBarras = produto.CodigoBarras.ToString();

            CategoriaProduto = produto.SubGrupoProduto.Grupo.Secao.Departamento.CategoriaProduto;
            DepartamentoProduto = produto.SubGrupoProduto.Grupo.Secao.Departamento;
            SecaoProduto = produto.SubGrupoProduto.Grupo.Secao;
            GrupoProduto = produto.SubGrupoProduto.Grupo;
            SubGrupoProduto = produto.SubGrupoProduto;
            Atendimentos = produto.Atendimentos.ToList();
        }

        public int Id { get; set; }
        [Required, StringLength(2000)]
        public string Nome { get; set; }
        [Required, StringLength(2000)]
        public string Descricao { get; set; }
        [Required, StringLength(150)]
        public string Codigo { get; set; }

        public string CodigoBarras { get; set; }
        public List<InformacaoProduto> Imagens { get; set; }
        public List<InformacaoProduto> Descricoes { get; set; }
        public List<AtendimentoProduto> Atendimentos { get; set; }
        public int Quantidade { get; set; }

        public CategoriaProduto CategoriaProduto { get; set; }
        public DepartamentoProduto DepartamentoProduto { get; set; }
        public SecaoProduto SecaoProduto { get; set; }
        public GrupoProduto GrupoProduto { get; set; }
        public SubGrupoProduto SubGrupoProduto { get; set; }
    }
}