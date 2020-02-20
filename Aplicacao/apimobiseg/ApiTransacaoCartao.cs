using System.Configuration;
using System.Web.Mvc;

namespace Aplicacao.apimobiseg
{
    public class ApiTransacaoCartao
    {
        public string ConsultaPessoa(long cpf)
        {
            var url = $"{ConfigurationManager.AppSettings["API_URL_PATH"]}{ConfigurationManager.AppSettings["API_URL_GET_PESSOA_DETALHE"].Replace("{fluxo}", ConfigurationManager.AppSettings["API_MOBISEG_FLUXO"] + "&").Replace("{chave}", ConfigurationManager.AppSettings["API_MOBISEG_CHAVE"] + "&").Replace("{cpf}", cpf + "")}";
            return ApiMobisegBase.RequestApi(HttpVerbs.Get, url, string.Empty);
        }

        public string RealizaTransacaoCartao(long cpf, long pessoa, string cartao, string validade, string cvv, string valor)
        {
            var url =
                $"{ConfigurationManager.AppSettings["API_URL_PATH"]}{ConfigurationManager.AppSettings["API_URL_POST_REALIZA_TRANSACAO_CARTAO"].Replace("{fluxo}", ConfigurationManager.AppSettings["API_MOBISEG_FLUXO"] + "&").Replace("{chave}", ConfigurationManager.AppSettings["API_MOBISEG_CHAVE"] + "&").Replace("{cpf}", cpf + "&").Replace("{plano}", ConfigurationManager.AppSettings["API_MOBISEG_PLANO"] + "&").Replace("{pessoa}", pessoa + "&").Replace("{cartao}", cartao + "&").Replace("{validade}", validade + "&").Replace("{cvv}", cvv + "&").Replace("{valor}", valor)}";
            return ApiMobisegBase.RequestApi(HttpVerbs.Get, url, string.Empty);
        }

        public string RealizaTransacaoCartaoOneClick(long pessoa, string cartao, string valor)
        {
            var url =
                $"{ConfigurationManager.AppSettings["API_URL_PATH"]}{ConfigurationManager.AppSettings["API_URL_POST_REALIZA_TRANSACAO_CARTAO"].Replace("{fluxo}", ConfigurationManager.AppSettings["API_MOBISEG_FLUXO"] + "&").Replace("{chave}", ConfigurationManager.AppSettings["API_MOBISEG_CHAVE"] + "&").Replace("{plano}", ConfigurationManager.AppSettings["API_MOBISEG_PLANO"] + "&").Replace("{pessoa}", pessoa + "&").Replace("{cartao}", cartao + "&").Replace("{valor}", valor)}";
            return ApiMobisegBase.RequestApi(HttpVerbs.Get, url, string.Empty);
        }
    }
}
