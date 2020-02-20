using System.Configuration;
using System.Web.Mvc;

namespace Aplicacao.ApiInfox
{
    public class ApiTransacaoCartao
    {
        public string Saldo(string idCliente, string cardNumber, string cardPass)
        {
            var url = $"{ConfigurationManager.AppSettings["API_INFOX_URL_PATH"]}{ConfigurationManager.AppSettings["API_URL_SALDO_CARTAO"].Replace("{idCliente}", idCliente).Replace("{cardNumber}", cardNumber).Replace("{cardPass}", cardPass)}";
            return ApiInfoxBase.RequestApi(HttpVerbs.Post, url, string.Empty);
        }

        public string Extrato(string idCliente, string cardNumber, string cardPass, string codEstabelecimento)
        {
            var url = $"{ConfigurationManager.AppSettings["API_INFOX_URL_PATH"]}{ConfigurationManager.AppSettings["API_URL_EXTRATO_CARTAO"].Replace("{idCliente}", idCliente).Replace("{cardNumber}", cardNumber).Replace("{cardPass}", cardPass).Replace("{codEstabelecimento}", codEstabelecimento)}";
            return ApiInfoxBase.RequestApi(HttpVerbs.Post, url, string.Empty);
        }

        public string CompraAVista(string json)
        {
            var url = $"{ConfigurationManager.AppSettings["API_INFOX_URL_PATH"]}{ConfigurationManager.AppSettings["API_URL_COMPRA_A_VISTA"]}";
            return ApiInfoxBase.RequestApi(HttpVerbs.Post, url, json);
        }

        public string CancelaCompraAVista(string json)
        {
            var url = $"{ConfigurationManager.AppSettings["API_INFOX_URL_PATH"]}{ConfigurationManager.AppSettings["API_URL_CANCELA_COMPRA_A_VISTA"]}";
            return ApiInfoxBase.RequestApi(HttpVerbs.Post, url, json);
        }

        public string DesfazCompraAVista(string json)
        {
            var url = $"{ConfigurationManager.AppSettings["API_INFOX_URL_PATH"]}{ConfigurationManager.AppSettings["API_URL_DESFAZ_COMPRA_A_VISTA"]}";
            return ApiInfoxBase.RequestApi(HttpVerbs.Post, url, json);
        }
    }
}
