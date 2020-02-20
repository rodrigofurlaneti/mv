using System.Collections.Generic;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEnderecoServico : IBaseServico<Endereco>
    {
        IList<Endereco> GetByPessoa(int pessoaId);
    }

    public class EnderecoServico : BaseServico<Endereco, IEnderecoRepositorio>, IEnderecoServico
    {
        public IList<Endereco> GetByPessoa(int pessoaId)
        {
            return BuscarPor(x => x.Pessoa.Id.Equals(pessoaId));
        }
    }
}