using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface ILojaAplicacao : IBaseAplicacao<Loja>
    {
        IEnumerable<LojaViewModel> Listar();
    }

    public class LojaAplicacao : BaseAplicacao<Loja, ILojaServico>, ILojaAplicacao
    {
        public IEnumerable<LojaViewModel> Listar()
        {
            return Servico.Buscar().Select(x => new LojaViewModel(x)).ToList();
        }

        public new void Salvar(Loja entity)
        {
            var lojaRetorno = BuscarPorId(entity.Id) ?? entity;

            lojaRetorno.Id = entity.Id;
            lojaRetorno.Descricao = entity.Descricao;
            lojaRetorno.RazaoSocial = entity.RazaoSocial;
            lojaRetorno.Cnpj = entity.Cnpj;
            lojaRetorno.InscricaoEstadual = entity.InscricaoEstadual;
            lojaRetorno.Telefone = entity.Telefone;
            lojaRetorno.Endereco = entity.Endereco;
            
            if (string.IsNullOrEmpty(lojaRetorno.Descricao)) throw new Exception("Campo Loja não preenchido!");
            if (string.IsNullOrEmpty(lojaRetorno.RazaoSocial)) throw new Exception("Campo Razão Social não preenchido!");
            if (lojaRetorno.Endereco == null || string.IsNullOrEmpty(lojaRetorno.Endereco.Cep)) throw new Exception("Campo CEP não preenchido!");

            Servico.Salvar(lojaRetorno);
        }
    }
}
