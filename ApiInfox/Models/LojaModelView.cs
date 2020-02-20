using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using Entidade;
using Entidade.Uteis;

namespace ApiInfox.Models
{
    public class LojaModelView
    {
        public LojaModelView(Loja loja)
        {
            CarregarDadosBasicos(loja);
        }

        public LojaModelView(Loja loja, float latitude, float longitude)
        {
            CarregarDadosBasicos(loja);

            if (string.IsNullOrWhiteSpace(loja.Endereco.Latitude) || string.IsNullOrWhiteSpace(loja.Endereco.Longitude))
                return;

            float lojaLat;
            if (!float.TryParse(loja.Endereco.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out lojaLat))
                return;
            float lojaLong;
            if (!float.TryParse(loja.Endereco.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out lojaLong))
                return;

            var localCoordinate = new GeoCoordinate(latitude, longitude);
            var lojaCoordinate = new GeoCoordinate(lojaLat, lojaLong);
            Distancia = localCoordinate.GetDistanceTo(lojaCoordinate);
        }

        private void CarregarDadosBasicos(Loja loja)
        {
            Id = loja.Id;
            Descricao = loja.Descricao;
            //Endereco = loja.Endereco;
            RazaoSocial = loja.RazaoSocial;
            InscricaoEstadual = loja.InscricaoEstadual;
            //Telefone = loja.Telefone;
            //Email = loja.Email;
            //Celular = loja.Celular;
            //Logo = loja.Logo();
            //Classificacao = loja.Classificacao;
            //NotaAvaliacao = loja.NotaAvaliacao;
            //LojaAprovada = loja.LojaAprovada;
            //HorarioFuncionamento = loja.HorarioFuncionamento;
            //HorarioDelivery = loja.HorarioDelivery;
            Cnpj = loja.Cnpj;
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        //public Endereco Endereco { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string InscricaoEstadual { get; set; }
        //public string Telefone { get; set; }
        //public string Celular { get; set; }
        //public string Email { get; set; }
        //public string Logo { get; set; }
        public double Distancia { get; set; }
        //public string Classificacao { get; set; }
        //public int NotaAvaliacao { get; set; }
        //public bool LojaAprovada { get; set; }
        //public IList<HorarioFuncionamentoLoja> HorarioFuncionamento { get; set; }
        //public IList<HorarioDeliveryLoja> HorarioDelivery { get; set; }
    }
}