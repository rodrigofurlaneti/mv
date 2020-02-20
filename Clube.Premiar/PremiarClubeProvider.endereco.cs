using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class EnderecoPremiarClubeProvider
    {
        protected PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }

        public EnderecoPremiarClubeProvider(
            PremiarClubeSettings clubeSettings
        )
        {
            ClubeSettings = clubeSettings;
            Api = new Api(clubeSettings);
        }

        public EnderecoPremiarClubeProvider(
            PremiarClubeSettings clubeSettings,
            Participant loggedUser
        )
        {
            ClubeSettings = clubeSettings;
            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public object Obter(long cep)
        {
            return Api.Get<object>($"addresses/{cep.ToString().PadLeft(8, '0')}");
        }
    }
}
