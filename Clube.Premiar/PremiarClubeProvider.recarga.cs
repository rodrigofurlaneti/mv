using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class RecargaPremiarClubeProvider
    {
        protected PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }

        public RecargaPremiarClubeProvider(
            PremiarClubeSettings clubeSettings,
            Participant loggedUser
        )
        {
            ClubeSettings = clubeSettings;
            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public MobileOperator[] Operadoras()
        {
            return Api.Get<MobileOperator[]>($"mobileRecharges/mobileOperator");
        }

        public AreaCode[] CodigosArea(string operadora)
        {
            return Api.Get<AreaCode[]>($"mobileRecharges/mobileOperator/{operadora}/areaCode");
        }

        public RechargeValue[] Valores(string operadora, int codigoArea)
        {
            return Api.Get<RechargeValue[]>($"mobileRecharges/mobileOperator/{operadora}/areaCode/{codigoArea}/rechargeValue");
        }
    }
}
