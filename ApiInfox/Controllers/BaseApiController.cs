using ApiInfox.AutorizadorFwCard;
using ApiInfox.Models;
using Core.Exceptions;
using System;
using System.Configuration;
using System.Web.Http;

namespace ApiInfox.Controllers
{
    public class BaseApiController : ApiController
    {
        protected string NSURequest
        {
            get
            {
                return ConfigurationManager.AppSettings["NSU_REQUEST"].ToString();
            }
        }
        protected string MasterKey
        {
            get
            {
                return ConfigurationManager.AppSettings["MASTER_KEY"].ToString();
            }
        }

        protected readonly IHostWebServservice _service;

        public BaseApiController()
        {
            _service = new IHostWebServservice();
        }

        protected void InicializacaoApiInfox(ref InicializacaoModelView inicializacao)
        {
            try
            {
                var CodResp = string.Empty;
                var NSU_Rede = string.Empty;
                var WorkKey = string.Empty;
                var ChaveAcessoTx = string.Empty;
                var DadosMensagem = string.Empty;
                var StrResp = string.Empty;

                var CodClienteNSU = $"{inicializacao.CodCliente}{inicializacao.NSU}";
                var chaveAcesso = $"{CodClienteNSU.Length.ToString("D4")}{CodClienteNSU}00000";
                inicializacao.ChaveAcesso = inicializacao.ChaveAcessoTx = Helpers.CryptoApiInfox.Encrypt(chaveAcesso, MasterKey);

                var dadosInicializacao = _service.Inicializacao(inicializacao.CodCliente,
                    inicializacao.ChaveAcesso.Replace("-", ""),
                    null,
                    ConfigurationManager.AppSettings["TIPO_CAPTURA"].ToString(),
                    null,
                    inicializacao.NSU,
                    null,
                    ref CodResp,
                    ref NSU_Rede,
                    ref WorkKey,
                    ref ChaveAcessoTx,
                    ref DadosMensagem,
                    ref StrResp);

                inicializacao.CodResp = CodResp;
                inicializacao.NSU_Rede = NSU_Rede;
                inicializacao.WorkKey = WorkKey;
                inicializacao.ChaveAcessoTx = ChaveAcessoTx;
                inicializacao.DadosMensagem = DadosMensagem;
                inicializacao.StrResp = StrResp;

                if (dadosInicializacao > 0)
                    throw new BusinessRuleException($"Retorno da Chamada Inicial: {StrResp}");
            }
            catch (BusinessRuleException br)
            {
                throw new BusinessRuleException($"Ocorreu um erro - [{br.Message}].");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
