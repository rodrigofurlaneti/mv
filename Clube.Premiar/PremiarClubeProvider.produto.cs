using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class ProdutoPremiarClubeProvider
    {
        protected PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }

        public ProdutoPremiarClubeProvider(
            PremiarClubeSettings clubeSettings,
            Participant loggedUser
        )
        {
            ClubeSettings = clubeSettings;

            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public Product Detalhe(string sku)
        {
            return Api.Get<Product>($"products/skus/{sku}");
        }

        public Category[] Categorias()
        {
            return Api.Get<Category[]>("products/categories");
        }

        public Search Buscar()
        {
            return Api.Get<Search>("products?sort=POPULARITY&_offset=0&_limit=10");
        }

        public Availability[] Disponibilidade(string sku)
        {
            return Api.Get<Availability[]>($"products/skus/{sku}/availability?vendorid=10957&originalid=4393509");
        }
    }
}
