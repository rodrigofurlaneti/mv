using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class CompraPremiarClubeProvider
    {
        protected PremiarClubeSettings ClubeSettings { get; set; }
        public Api Api { get; protected set; }

        public CompraPremiarClubeProvider(
            PremiarClubeSettings clubeSettings,
            Participant loggedUser
        )
        {
            ClubeSettings = clubeSettings;
            Api = new AuthenticatedApi(clubeSettings, loggedUser);
        }

        public object Obter(int purchaseId) {
            return Api.Get<object>($"purchases/{purchaseId}");
        }

        public object ObterTodas() {
            return Api.Get<object>($"purchases/me");
        }

        public object Criar() {
            return Api.Post<object, dynamic>($"purchases", new { channelType = "ONLINE" });
        }

        public object CriarParaRecarga  () {
            return Api.Post<object, dynamic>($"purchases/mobileRecharge", new
            {
                mobileOperator = "Claro",
                areaCode = "11",
                mobileNumber = "000000000",
                serviceId = "e317765286394dcaa43c"
            });
        }

        public object CriarParaContas() {
            return Api.Post<object, dynamic>($"purchases/billPayment", new
            {
                barCode = "846400000010401500060017711361010947201711150000",
                dueDate = "2018-05-07T00:00:00-02:00",
                documentNumber = "11111111111",
                serviceId = "c8bcf96e95c14988937359a265"
            });
        }

        public object CriarParaDinheiro() {
            return Api.Post<object, dynamic>($"purchases", new
            {
                channelType = "ONLINE",
                creditCardPayment = new
                {
                    cardBrandId = 1,
                    cardHolderName = "NOME CARTAO",
                    cardNumber = "0000000000000000",
                    expirationMonth = 10,
                    expirationYear = 25,
                    securityCode = "123",
                    installments = 1,
                    pointsValue = 1
                }
            });
        }

        public object Autorizar() {
            return Api.Post<object, dynamic>($"purchases/authorize", new
            {
                SendEmail = false,
                SendSMS = false,
                channelType = "ONLINE",
                Shipping = (object)null,
                Items = new object[]{
                    new { ProductId = "243523452", VendorSku = "567457", Sku = "52345234", Name = "joao@silva.com", Quantity= 1, Category= "Aereo", VendorId= "21071", CategoryId= 0, CostPrice= 1, SellingPrice= 1, Parameters = (object)null }
                },
                ConversionRate = new object[]{
                    new { VendorId= "21071", ConversionFactor= 1 }
                },
                Payment = new object[]{
                    new { Value= 1, Type= "Points" }
                }
            });
        }

        public object Confirmar() {
            return Api.Put<object, dynamic>($"purchases/{"11353212"}/confirm", new {
                confirmOrderRequest = new object[] {
                    new { orderId = 0000, status = true }
                }
            });
        }
    }
}
