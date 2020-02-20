using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;
using Aplicacao.ViewModels;
using System.Linq;

namespace Aplicacao
{
    public interface IMuralAplicacao : IBaseAplicacao<Mural>
    {
        IEnumerable<MuralViewModel> Listar();
    }

    public class MuralAplicacao : BaseAplicacao<Mural, IMuralServico>, IMuralAplicacao
    {
        private readonly IMuralServico _servico;
        public MuralAplicacao(IMuralServico servico)
        {
            _servico = servico;
        }

        public new void Salvar(Mural entity)
        {
            var muralRetorno = BuscarPorId(entity.Id) ?? entity;

            muralRetorno.Id = entity.Id;

            muralRetorno.Titulo = entity.Titulo;
            muralRetorno.DataPublicacao = entity.DataPublicacao;
            muralRetorno.FotoCapa = entity.FotoCapa;
            muralRetorno.Descricao = entity.Descricao;
            muralRetorno.Facebook = entity.Facebook;

            if (string.IsNullOrEmpty(muralRetorno.Descricao)) throw new Exception("Campo Descrição não preenchido!");
            if (string.IsNullOrEmpty(muralRetorno.FotoCapa)) throw new Exception("Campo Foto de Capa não preenchido!");
            if (string.IsNullOrEmpty(muralRetorno.Facebook)) throw new Exception("Campo Facebook não preenchido!");

            Servico.Salvar(muralRetorno);
        }

        public IEnumerable<MuralViewModel> Listar()
        {
            return _servico.Buscar().Select(x => new MuralViewModel(x)).ToList();
        }
    }
}