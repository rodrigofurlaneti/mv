using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Aplicacao.apiebanx
{
    public class ApiTransacaoCartao
    {
        private static readonly string API_EBANX_INTEGRATION_KEY = ConfigurationManager.AppSettings["API_EBANX_INTEGRATION_KEY"];
        private static readonly string API_EBANX_PUBLIC_KEY = ConfigurationManager.AppSettings["API_EBANX_PUBLIC_KEY"];

        public Models.DirectResponseModel CompraDireta(Entidade.Pedido pedido, bool auto_capture = true)
        {
            var usuario = pedido.Usuario.Pessoa;
            var cartao = new Models.CreditCardModel
            {
                card_number = pedido.Cartao.NumeroSemMascara,
                card_name = pedido.Cartao.NomeImpresso ?? usuario.Nome.ToUpper(),
                card_due_date = pedido.Cartao.DataValidade?.ToString("MM/yyyy"),
                card_cvv = pedido.Cartao.CvvSemMascara
            };

            if (!string.IsNullOrEmpty(pedido.Cartao.Token))
                cartao = new Models.CreditCardModel { token = pedido.Cartao.Token };

            cartao.auto_capture = auto_capture;

            var endereco = usuario.EnderecoResidencial;
            var request = new Models.DirectRequestModel
            {
                integration_key = API_EBANX_INTEGRATION_KEY,
                operation = "request",
                mode = "full",
                payment = new Models.PaymentModel
                {
                    name = usuario.Nome,
                    email = usuario.Email,
                    document = usuario.Cpf ?? pedido.Usuario.Pessoa.Cnpj,
                    address = endereco?.Logradouro,
                    street_number = endereco?.Numero,
                    city = endereco?.Cidade?.Descricao,
                    state = endereco?.Cidade?.Estado?.Sigla,
                    zipcode = endereco?.Cep.ExtractNumbers().PadLeft(8, '0'),
                    country = "br",
                    phone_number = usuario.Celular,
                    payment_type_code = "discover",
                    merchant_payment_code = pedido.Loja.Cnpj.ExtractNumbers().PadLeft(14, '0') + "_" + pedido.Id,
                    currency_code = "BRL",
                    instalments = 1,
                    amount_total = pedido.Valor,
                    creditcard = cartao
                }
            };

            var url = $"/ws/direct";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var response = ApiEbanxBase.RequestApi(HttpVerbs.Post, url, json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.DirectResponseModel>(response);
        }

        public Models.CancelResponseModel Cancelar(Entidade.Pedido pedido)
        {
            var hash = pedido.ListaHistorico.FirstOrDefault(h => h.StatusPedido == Entidade.Uteis.StatusPedido.AguardandoPagamento)?.CodigoRetornoTransacao;

            var request = new Models.CancelRequestModel
            {
                integration_key = API_EBANX_INTEGRATION_KEY,
                hash = hash
            };

            var url = $"/ws/cancel";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var response = ApiEbanxBase.RequestApi(HttpVerbs.Post, url, json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.CancelResponseModel>(response);
        }

        public Models.RefundResponseModel Estornar(Entidade.Pedido pedido, string motivo)
        {
            var hash = pedido.ListaHistorico.FirstOrDefault(h => h.StatusPedido == Entidade.Uteis.StatusPedido.AguardandoPagamento)?.CodigoRetornoTransacao;
            var cnpj = pedido.Loja.Cnpj.ExtractNumbers().PadLeft(14, '0');
            var request = new Models.RefundRequestModel
            {
                integration_key = API_EBANX_INTEGRATION_KEY,
                operation = "request",
                hash = hash,
                amount = pedido.Valor / 100,
                description = motivo,
                merchant_refund_code = $"{cnpj}_{pedido.Id}_refund"
            };

            var url = $"/ws/refund";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var response = ApiEbanxBase.RequestApi(HttpVerbs.Post, url, json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.RefundResponseModel>(response);
        }

        public Models.RefundOrCancelResponseModel CancelarOuEstornar(Entidade.Pedido pedido, string motivo)
        {
            var hash = pedido.ListaHistorico.FirstOrDefault(h => h.StatusPedido == Entidade.Uteis.StatusPedido.AguardandoPagamento)?.CodigoRetornoTransacao;
            var cnpj = pedido.Loja.Cnpj.ExtractNumbers().PadLeft(14, '0');
            var request = new Models.RefundOrCancelRequestModel
            {
                integration_key = API_EBANX_INTEGRATION_KEY,
                hash = hash,
                description = motivo,
                merchant_refund_code = $"{cnpj}_{pedido.Id}_refund"
            };

            var url = $"/ws/refundOrCancel";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var response = ApiEbanxBase.RequestApi(HttpVerbs.Post, url, json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.RefundOrCancelResponseModel>(response);
        }

        public string Imprimir(Entidade.Pedido pedido)
        {
            var hash = pedido.ListaHistorico.FirstOrDefault(h => h.StatusPedido == Entidade.Uteis.StatusPedido.AguardandoPagamento)?.CodigoRetornoTransacao;
            var url = $"{ApiEbanxBase.API_EBANX_URL.TrimEnd('/')}/print?hash={hash}&format=html";

            var response = new System.Net.WebClient().DownloadString(url);
            return response;
        }

        public Models.CaptureResponseModel Capturar(Entidade.Pedido pedido, decimal? valor = null)
        {
            var hash = pedido.ListaHistorico.FirstOrDefault(h => h.StatusPedido == Entidade.Uteis.StatusPedido.AguardandoPagamento)?.CodigoRetornoTransacao;

            var request = new Models.CaptureRequestModel
            {
                integration_key = API_EBANX_INTEGRATION_KEY,
                hash = hash
            };

            if (valor.HasValue) request.amount = valor.Value;

            var url = $"/ws/capture";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var response = ApiEbanxBase.RequestApi(HttpVerbs.Post, url, json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.CaptureResponseModel>(response);
        }

        public Models.QueryResponseModel Consulta(Entidade.Pedido pedido)
        {
            var hash = pedido.ListaHistorico.FirstOrDefault(h => h.StatusPedido == Entidade.Uteis.StatusPedido.AguardandoPagamento)?.CodigoRetornoTransacao;

            var request = new Models.QueryRequestModel
            {
                integration_key = API_EBANX_INTEGRATION_KEY,
                hash = hash
            };

            var url = $"/ws/query";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var response = ApiEbanxBase.RequestApi(HttpVerbs.Post, url, json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.QueryResponseModel>(response);
        }

        public Models.QueryResponseModel Consulta(string hash)
        {
            var url = $"/ws/query?integration_key={API_EBANX_INTEGRATION_KEY}&hash={hash}";
            var json = "";

            var response = ApiEbanxBase.RequestApi(HttpVerbs.Post, url, json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.QueryResponseModel>(response);
        }

        public Models.TokenResponseModel TokenCartao(Entidade.Cartao cartao)
        {
            var request = new Models.TokenRequestModel
            {
                integration_key = API_EBANX_INTEGRATION_KEY,
                public_integration_key = API_EBANX_PUBLIC_KEY,
                payment_type_code = "discover",
                country = "br",
                creditcard = new Models.CreditCardModel
                {
                    card_number = cartao.NumeroSemMascara,
                    card_name = cartao.NomeImpresso,
                    card_due_date = cartao.DataValidade?.ToString("MM/yyyy"),
                    card_cvv = cartao.CvvSemMascara
                }
            };
            
            var url = $"/ws/token";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var response = ApiEbanxBase.RequestApi(HttpVerbs.Post, url, json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.TokenResponseModel>(response);
        }
    }
}
