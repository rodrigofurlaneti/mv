using Aplicacao.Base;
using Dominio;
using Entidade;
using System;

namespace Aplicacao
{
    public interface IFornecedorAplicacao : IBaseAplicacao<Fornecedor>
    {
    }

    public class FornecedorAplicacao : BaseAplicacao<Fornecedor, IFornecedorServico>, IFornecedorAplicacao
    {
        private readonly IFornecedorServico _fornecedorServico;
        private readonly ICidadeAplicacao _cidadeAplicacao;

        public FornecedorAplicacao(ICidadeAplicacao cidadeAplicacao, IFornecedorServico fornecedorServico)
        {
            _cidadeAplicacao = cidadeAplicacao;
            _fornecedorServico = fornecedorServico;
        }

        public new void Salvar(Fornecedor entity)
        {
            var lojaRetorno = BuscarPorId(entity.Id) ?? entity;

            lojaRetorno.Id = entity.Id;
            lojaRetorno.Descricao = entity.Descricao;
            lojaRetorno.RazaoSocial = entity.RazaoSocial;
            lojaRetorno.Cnpj = entity.Cnpj;
            lojaRetorno.InscricaoEstadual = entity.InscricaoEstadual;
            lojaRetorno.Telefone = entity.Telefone;
            lojaRetorno.Endereco = entity.Endereco;

            if (string.IsNullOrEmpty(lojaRetorno.Descricao)) throw new Exception("Campo Descricão não preenchido!");
            if (string.IsNullOrEmpty(lojaRetorno.RazaoSocial)) throw new Exception("Campo Razão Social não preenchido!");
            if (lojaRetorno.Endereco == null || string.IsNullOrEmpty(lojaRetorno.Endereco.Cep)) throw new Exception("Campo CEP não preenchido!");

            Servico.Salvar(lojaRetorno);
        }
    }
}
