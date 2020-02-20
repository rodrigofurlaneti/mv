using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class PagamentoPremiarClubeProvider
    {
        protected PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }

        public PagamentoPremiarClubeProvider(
            PremiarClubeSettings clubeSettings,
            Participant loggedUser
        )
        {
            ClubeSettings = clubeSettings;
            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public object[] Fornecedores()
        {
            return Api.Get<object[]>($"billPayments/vendor");
        }

        public object[] Servicos(int vendorId)
        {
            return Api.Get<object[]>($"billPayments/vendor/{vendorId}/service");
        }

        public object Validar(int vendorId, string serviceId, string barcode)
        {
            return Api.Get<object>($"billpayments/vendor/{vendorId}/service/{serviceId}/validate/{barcode}");
        }
    }
}
