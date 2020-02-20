using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clube.Premiar.Models;

namespace Clube.Premiar
{
    public class CarrinhoPremiarClubeProvider
    {
        protected PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }
        
        public CarrinhoPremiarClubeProvider(
            PremiarClubeSettings clubeSettings,
            Participant loggedUser
        )
        {
            ClubeSettings = clubeSettings;

            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public Cart Obter()
        {
            return Api.Get<Cart>("carts/me");
        }

        public ShippingRateVendor ObterFrete(long cep)
        {
            return Api.Get<ShippingRateVendor>($"carts/me/shipping/rates/{cep}");
        }

        public void AdicionarItem(int vendorId, string sku)
        {
            Api.Post<object, object>("carts/me/items", new { vendorId, sku });
        }

        public void AlterarItem(int vendorId, string sku, int quantity)
        {
            Api.Put<object, object>($"carts/me/items/{sku}", new { vendorId, quantity });
        }

        public void ExcluirItem(string sku)
        {
            Api.Delete<object>($"carts/me/items/{sku}");
        }

        public void SalvarEndereco(ShippingCustomer shippingCustomer)
        {
            Api.Put<ShippingCustomer, object>("carts/me/shipping", shippingCustomer);
        }
    }

    public partial class ShippingRate
    {
        public long Total { get; set; }
        public long TotalCash { get; set; }
        public ShippingRateVendor[] Vendors { get; set; }
    }

    public partial class ShippingRateVendor
    {
        public long Id { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public long Total { get; set; }
        public long TotalCash { get; set; }
    }

}
