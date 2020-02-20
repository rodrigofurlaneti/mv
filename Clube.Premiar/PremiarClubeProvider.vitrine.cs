using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class AuthenticatedVitrinePremiarClubeProvider
        : VitrinePremiarClubeProvider
    {
        public AuthenticatedVitrinePremiarClubeProvider(
            PremiarClubeSettings clubeSettings,
            Models.Participant loggedUser
        )
            : base(clubeSettings)
        {
            ClubeSettings = clubeSettings;

            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public Search Obter()
        {
            return Api.Get<Search>("showcases/home?_offset=0");
        }
    }

    public class VitrinePremiarClubeProvider
    {
        protected PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }

        public VitrinePremiarClubeProvider(
            PremiarClubeSettings clubeSettings
        )
        {
            ClubeSettings = clubeSettings;

            Api = new Api(clubeSettings);
        }

        public Search ObterPorCampanha()
        {
            return Api.Get<Search>("showcases/campaign?_offset=0");
        }

        public Search ObterPorCatalogo(int catalogId)
        {
            return Api.Get<Search>($"showcases/home/withcatalog?_offset=0&catalogId={catalogId}");
        }
    }
}
