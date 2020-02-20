using System.Collections.Generic;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IDescontoPessoaServico : IBaseServico<DescontoPessoa>
    {
        void Salvar(IList<DescontoPessoa> descontos);
        void ExcluirPorDesconto(int id);
    }

    public class DescontoPessoaServico : BaseServico<DescontoPessoa, IDescontoPessoaRepositorio>, IDescontoPessoaServico
    {
        private readonly IDescontoPessoaRepositorio _descontoPessoaRepositorio;

        public DescontoPessoaServico(IDescontoPessoaRepositorio descontoPessoaRepositorio)
        {
            _descontoPessoaRepositorio = descontoPessoaRepositorio;
        }

        public void Salvar(IList<DescontoPessoa> descontos)
        {
            _descontoPessoaRepositorio.Save(descontos);
        }

        public void ExcluirPorDesconto(int id)
        {
            _descontoPessoaRepositorio.DeleteByQuery($"delete from DescontoPessoa where Desconto = {id}");
        }
    }
}