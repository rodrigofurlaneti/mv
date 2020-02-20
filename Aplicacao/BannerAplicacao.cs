using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;
using Aplicacao.ViewModels;
using System.Linq;

namespace Aplicacao
{
    public interface IBannerAplicacao : IBaseAplicacao<Banner>
    {
        IEnumerable<BannerViewModel> Listar();
    }

    public class BannerAplicacao : BaseAplicacao<Banner, IBannerServico>, IBannerAplicacao
    {
        private readonly IBannerServico _servico;
        public BannerAplicacao(IBannerServico servico)
        {
            _servico = servico;
        }

        public new void Salvar(Banner entity)
        {
            var bannerRetorno = BuscarPorId(entity.Id) ?? entity;

            bannerRetorno.Id = entity.Id;
            bannerRetorno.URL = entity.URL;
            bannerRetorno.DataInicio = entity.DataInicio;
            bannerRetorno.DataFim = entity.DataFim;
            bannerRetorno.TipoBanner = entity.TipoBanner;

            if (string.IsNullOrEmpty(bannerRetorno.URL)) throw new Exception("Campo URL não preenchido!");
            if (bannerRetorno.DataInicio == DateTime.MinValue || bannerRetorno.DataFim == DateTime.MaxValue) throw new Exception("Campo Data inicio não preenchido!");
            if (bannerRetorno.DataFim == DateTime.MinValue || bannerRetorno.DataFim == DateTime.MaxValue) throw new Exception("Campo Data fim não preenchido!");
            if (bannerRetorno.DataInicio >= bannerRetorno.DataFim) throw new Exception("Data inicio deve ser menor que data fim!");
            if ((int)bannerRetorno.TipoBanner == 0) throw new Exception("Campo Tipo não preenchido!");


            Servico.Salvar(bannerRetorno);
        }

        public IEnumerable<BannerViewModel> Listar()
        {
            return _servico.Buscar().Select(x => new BannerViewModel(x)).ToList();
        }
    }
}