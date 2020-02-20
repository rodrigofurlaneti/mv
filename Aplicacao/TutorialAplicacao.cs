using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;
using Aplicacao.ViewModels;
using System.Linq;

namespace Aplicacao
{
    public interface ITutorialAplicacao : IBaseAplicacao<Tutorial>
    {
        IEnumerable<TutorialViewModel> Listar();
    }

    public class TutorialAplicacao : BaseAplicacao<Tutorial, ITutorialServico>, ITutorialAplicacao
    {
        private readonly ITutorialServico _servico;
        public TutorialAplicacao(ITutorialServico servico)
        {
            _servico = servico;
        }

        public new void Salvar(Tutorial entity)
        {
            var tutorialRetorno = BuscarPorId(entity.Id) ?? entity;

            tutorialRetorno.Id = entity.Id;
            tutorialRetorno.URL = entity.URL;

            if (string.IsNullOrEmpty(tutorialRetorno.URL)) throw new Exception("Campo URL não preenchido!");

            Servico.Salvar(tutorialRetorno);
        }

        public IEnumerable<TutorialViewModel> Listar()
        {
            return _servico.Buscar().Select(x => new TutorialViewModel(x)).ToList();
        }
        
    }
}