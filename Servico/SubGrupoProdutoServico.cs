using System.Collections.Generic;
using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ISubGrupoProdutoServico : IBaseServico<SubGrupoProduto>
    {
	    IList<SubGrupoProduto> GetSubGruposPorGrupo(int grupoId);
    }

    public class SubGrupoProdutoServico : BaseServico<SubGrupoProduto, ISubGrupoProdutoRepositorio>, ISubGrupoProdutoServico
    {
        private static void Validar(SubGrupoProduto entity)
        {
            if (entity == null)
                throw new System.ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(entity.Nome))
                throw new BusinessRuleException("Informe o Nome!");
            if (entity.Grupo.Id == 0)
                throw new BusinessRuleException("Selecione um Grupo!");
        }

        public new void Salvar(SubGrupoProduto entidade)
        {
            Validar(entidade);

            Repositorio.Save(entidade);
        }

	    public IList<SubGrupoProduto> GetSubGruposPorGrupo(int grupoId)
	    {
		    return Repositorio.ListBy(sg => sg.Grupo.Id == grupoId);
	    }
    }
}