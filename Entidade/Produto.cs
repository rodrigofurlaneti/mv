using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Entidade.Base;

namespace Entidade
{
    public class Produto : IEntity
    {
        private CategoriaProduto _categoriaProduto;
        private DepartamentoProduto _departamentoProduto;

        public Produto()
        {
            Lojas = new List<Loja>();
            Informacoes = new List<InformacaoProduto>();
            DataInsercao = DateTime.Now;
        }

        public virtual int Id { get; set; }
        public virtual DateTime DataInsercao { get; set; }

        [Required, StringLength(2000)]
        public virtual string Nome { get; set; }
        [Required, StringLength(2000)]
        public virtual string Descricao { get; set; }
        [Required, StringLength(150)]
        public virtual string Codigo { get; set; }

        public virtual long CodigoBarras { get; set; }

        public virtual IList<InformacaoProduto> Informacoes { get; set; }
        public virtual IList<AtendimentoProduto> Atendimentos { get; set; }
        public virtual IList<ProdutoPreco> Precos { get; set; }

        [IgnoreDataMember]
        public virtual IList<Loja> Lojas { get; set; }

        public virtual int CodigoPcSist { get; set; }
        public virtual string CodigoAuxiliarPcSist { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual CategoriaProduto CategoriaProduto { get => _categoriaProduto; set => _categoriaProduto = value; }

        public virtual DepartamentoProduto DepartamentoProduto { get => _departamentoProduto; set => _departamentoProduto = value; }
        
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as Produto;
            if (t == null) return false;
            if (Id == t.Id
             && CodigoBarras == t.CodigoBarras)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ Id.GetHashCode();
            hash = (hash * 397) ^ CodigoBarras.GetHashCode();

            return hash;
        }
        #endregion

    }
}