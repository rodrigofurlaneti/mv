using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class AuthenticatedSettingsPremiarClubeProvider
        : SettingsPremiarClubeProvider
    {
        public AuthenticatedSettingsPremiarClubeProvider(
            PremiarClubeSettings clubeSettings,
            Participant loggedUser
        ) 
            : base(clubeSettings)
        {
            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public Catalog Catalogo()
        {
            return Api.Get<Catalog>($"configurations/catalogs/byToken");
        }

        public CardBrand[] BandeirasCartao()
        {
            return Api.Get<CardBrand[]>($"configurations/cardBrands");
        }
    }

    public class SettingsPremiarClubeProvider
    {
        protected PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }

        public SettingsPremiarClubeProvider(
            PremiarClubeSettings clubeSettings
        )
        {
            ClubeSettings = clubeSettings;
            Api = new Api(clubeSettings);
        }

        public Catalog Catalogo(int catalogId)
        {
            return Api.Get<Catalog>($"configurations/catalogs/{catalogId}");
        }
    }
}
