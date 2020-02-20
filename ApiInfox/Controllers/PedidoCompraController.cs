using ApiInfox.Models;
using System.Configuration;
using System.Web.Http;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/pedidocompra")]
    public class PedidoCompraController : BaseApiController
    {
        [HttpPost]
        [Route("compra/avista")]
        public virtual ResponseModelView CompraAVista([FromBody] PedidoCompraModelView pedido)
        {
            var inicializacao = new InicializacaoModelView
            {
                CodCliente = pedido.Cliente.Id,
                NSU = ConfigurationManager.AppSettings["NSU"].ToString()
            };

            InicializacaoApiInfox(ref inicializacao);

            return CompraAVista(ref inicializacao, ref pedido);
        }

        [HttpPost]
        [Route("cancela/compra/avista")]
        public virtual ResponseModelView CancelaCompraAVista([FromBody] PedidoCompraModelView pedido)
        {
            var inicializacao = new InicializacaoModelView
            {
                CodCliente = pedido.Cliente.Id,
                NSU = ConfigurationManager.AppSettings["NSU"].ToString()
            };

            InicializacaoApiInfox(ref inicializacao);

            return CancelaCompraAVista(ref inicializacao, ref pedido);
        }

        [HttpPost]
        [Route("desfaz/compra/avista")]
        public virtual ResponseModelView DesfazCompraAVista([FromBody] PedidoCompraModelView pedido)
        {
            var inicializacao = new InicializacaoModelView
            {
                CodCliente = pedido.Cliente.Id,
                NSU = ConfigurationManager.AppSettings["NSU"].ToString()
            };

            InicializacaoApiInfox(ref inicializacao);

            return DesfazCompraAVista(ref inicializacao, ref pedido);
        }

        private ResponseModelView CompraAVista(ref InicializacaoModelView inicializacao, ref PedidoCompraModelView pedido)
        {
            var CodResp = string.Empty;
            var NSU_Rede = string.Empty;
            var StrResp = string.Empty;
            var DadosMensagem = string.Empty;
            var cartaoCVVEncrypted = "";



            var workKeyData = Helpers.CryptoApiInfox.DecryptApi(inicializacao.WorkKey, MasterKey);
            var chaveAcessoChamadaConcatenada = Helpers.CryptoApiInfox.EncryptApi($"{pedido.Cliente.Id}{NSURequest}{Helpers.CryptoApiInfox.DecryptApi(inicializacao.ChaveAcessoTx, workKeyData)}", workKeyData);

            var CPFEncrypted = Helpers.CryptoApiInfox.EncryptApi(pedido.Cliente.Pessoa.Cpf, workKeyData);
            var DataNascimentoEncrypted = Helpers.CryptoApiInfox.EncryptApi(pedido.Cliente.Pessoa.DataNascimento.ToShortDateString(), workKeyData);
            var cartaoNumeroEncrypted = Helpers.CryptoApiInfox.EncryptApi(pedido.Cliente.Cartao.Numero, workKeyData);
            var cartaoSenhaEncrypted = Helpers.CryptoApiInfox.EncryptApi(pedido.Cliente.Cartao.Senha, workKeyData);
            if (pedido.Cliente.Cartao.Cvv != null)
            {
                 cartaoCVVEncrypted = Helpers.CryptoApiInfox.EncryptApi(pedido.Cliente.Cartao.Cvv, workKeyData);
            }
      
            var cartaoNomeImpressoEncrypted = Helpers.CryptoApiInfox.EncryptApi(pedido.Cliente.Cartao.NomeImpresso, workKeyData);

            var dados = _service.CompraVista(chaveAcessoChamadaConcatenada,
                pedido.CodEstabelecimento,
                ConfigurationManager.AppSettings["TIPO_CAPTURA"].ToString(),
                null,
                cartaoNumeroEncrypted,
                pedido.ValorPedido,
                NSURequest,
                null,
                cartaoSenhaEncrypted,
                cartaoCVVEncrypted,
                CPFEncrypted,
                DataNascimentoEncrypted,
                cartaoNomeImpressoEncrypted,
                ref CodResp,
                ref NSU_Rede,
                ref DadosMensagem,
                ref StrResp);

            return new ResponseModelView
            {
                Return = dados,
                CodResp = CodResp,
                NSU_Rede = NSU_Rede,
                NSU_RedeRX = NSU_Rede,
                StrResp = StrResp,
                DadosMensagem = DadosMensagem
            };
        }
        private ResponseModelView CancelaCompraAVista(ref InicializacaoModelView inicializacao, ref PedidoCompraModelView pedido)
        {
            var CodResp = string.Empty;
            var NSU_Rede = string.Empty;
            var NSU_RedeRX = string.Empty;
            var StrResp = string.Empty;
            var DadosMensagem = string.Empty;

            var workKeyData = Helpers.CryptoApiInfox.DecryptApi(inicializacao.WorkKey, MasterKey);
            var chaveAcessoChamadaConcatenada = Helpers.CryptoApiInfox.EncryptApi($"{pedido.Cliente.Id}{NSURequest}{Helpers.CryptoApiInfox.DecryptApi(inicializacao.ChaveAcessoTx, workKeyData)}", workKeyData);

            var cartaoNumeroEncrypted = Helpers.CryptoApiInfox.EncryptApi(pedido.Cliente.Cartao.Numero, workKeyData);

            NSU_RedeRX = pedido.NSU_RedeRX;

            var dados = _service.CancelaCompraVista(chaveAcessoChamadaConcatenada,
                pedido.CodEstabelecimento,
                ConfigurationManager.AppSettings["TIPO_CAPTURA"].ToString(),
                null,
                cartaoNumeroEncrypted,
                NSURequest,
                NSU_RedeRX,
                ref CodResp,
                ref NSU_Rede,
                ref DadosMensagem,
                ref StrResp);

            return new ResponseModelView
            {
                Return = dados,
                CodResp = CodResp,
                NSU_Rede = NSU_Rede,
                NSU_RedeRX = NSU_RedeRX,
                StrResp = StrResp,
                DadosMensagem = DadosMensagem
            };
        }
        private ResponseModelView DesfazCompraAVista(ref InicializacaoModelView inicializacao, ref PedidoCompraModelView pedido)
        {
            var CodResp = string.Empty;
            var NSU_Rede = string.Empty;
            var NSU_RedeRX = string.Empty;
            var StrResp = string.Empty;
            var DadosMensagem = string.Empty;

            var workKeyData = Helpers.CryptoApiInfox.DecryptApi(inicializacao.WorkKey, MasterKey);
            var chaveAcessoChamadaConcatenada = Helpers.CryptoApiInfox.EncryptApi($"{pedido.Cliente.Id}{NSURequest}{Helpers.CryptoApiInfox.DecryptApi(inicializacao.ChaveAcessoTx, workKeyData)}", workKeyData);

            var cartaoNumeroEncrypted = Helpers.CryptoApiInfox.EncryptApi(pedido.Cliente.Cartao.Numero, workKeyData);

            NSU_RedeRX = pedido.NSU_RedeRX;

            var dados = _service.DesfazCompraVista(chaveAcessoChamadaConcatenada,
                pedido.CodEstabelecimento,
                ConfigurationManager.AppSettings["TIPO_CAPTURA"].ToString(),
                null,
                cartaoNumeroEncrypted,
                NSURequest,
                NSU_RedeRX,
                ref CodResp,
                ref NSU_Rede,
                ref DadosMensagem,
                ref StrResp);

            return new ResponseModelView
            {
                Return = dados,
                CodResp = CodResp,
                NSU_Rede = NSU_Rede,
                NSU_RedeRX = NSU_RedeRX,
                StrResp = StrResp,
                DadosMensagem = DadosMensagem
            };
        }
    }
}
