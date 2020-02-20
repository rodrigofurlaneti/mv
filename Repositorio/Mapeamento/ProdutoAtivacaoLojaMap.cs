using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class ProdutoAtivacaoLojaMap : ClassMap<ProdutoAtivacaoLoja>
    {
        public ProdutoAtivacaoLojaMap()
        {
            Table("ProdutoAtivacaoLoja");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DataInsercao).Not.Update();
            Map(x => x.ProdutoAlimentacao);
            Map(x => x.ProdutoRefeicao);
            Map(x => x.ProdutoCombustivel);
            Map(x => x.ProdutoAdiantamento);
            Map(x => x.ProdutoFarmacia);
            Map(x => x.TaxaRefeicao).Column("TaxaRefeicao").Length(10).Precision(8).Scale(2);
            Map(x => x.TaxaAlimentacao).Column("TaxaAlimentacao").Length(10).Precision(8).Scale(2);
            Map(x => x.TaxaCombustivel).Column("TaxaCombustivel").Length(10).Precision(8).Scale(2);
            Map(x => x.TaxaAdiantamento).Column("TaxaAdiantamento").Length(10).Precision(8).Scale(2);
        }
    }
}
