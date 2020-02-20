using System.Collections.Generic;
using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IGrupoProdutoServico : IBaseServico<GrupoProduto>
    {
	    IList<GrupoProduto> GetGruposPorSecao(int secaoId);
	    IList<GrupoProduto> GetGruposPorCategoria(int categoriaId);
    }

    public class GrupoProdutoServico : BaseServico<GrupoProduto, IGrupoProdutoRepositorio>, IGrupoProdutoServico
    {
        private static void Validar(GrupoProduto entity)
        {
            if (entity == null)
                throw new System.ArgumentNullException(nameof(entity));

            if (string.IsNullOrEmpty(entity.Nome))
                throw new BusinessRuleException("Informe o nome do grupo!");

            if (entity.Secao.Id == 0)
                throw new BusinessRuleException("Selecione uma seção!");
        }

        public new void Salvar(GrupoProduto entidade)
        {
            Validar(entidade);

            Repositorio.Save(entidade);
        }

	    public IList<GrupoProduto> GetGruposPorSecao(int secaoId)
	    {
		    return Repositorio.ListBy(g => g.Secao.Id == secaoId);
	    }

	    public IList<GrupoProduto> GetGruposPorCategoria(int categoriaId)
	    {
		    return Repositorio.ListBy(g => g.Secao.Departamento.CategoriaProduto.Id == categoriaId);
	    }
	}
}