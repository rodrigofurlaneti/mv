using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;
using PedidoVoucher = Entidade.PedidoVoucher;

namespace Dominio
{
    public interface IPedidoVoucherServico : IBaseServico<PedidoVoucher>
    {
        PedidoVoucher ValidarESalvar(PedidoVoucher entity);
        void Retirar(int id, string code, Usuario usuario);
        void AtribuiStatus(int id, int status, Usuario usuario, string descricao = null, string codigoRetornoTransacao = null);
        void ModificaStatus(int id, StatusPedido status, Usuario usuario, string descricao = null, string codigoRetornoTransacao = null);
    }

    public class PedidoVoucherServico : BaseServico<PedidoVoucher, IPedidoVoucherRepositorio>, IPedidoVoucherServico
    {
        public PedidoVoucher ValidarESalvar(PedidoVoucher pedido)
        {
            Valida(pedido);
            
            pedido.AddListaHistorico(StatusPedido.AguardandoConfirmacao, pedido.Usuario, pedido);
            pedido.Id = Repositorio.SaveAndReturn(pedido);
            pedido = Repositorio.GetById(pedido.Id);

            var qrcodeServico = ServiceLocator.Current.GetInstance<IQrCodeServico>();
            pedido.QrCode = qrcodeServico.GeraUrlESalva(pedido);
            var code = Tools.GenerateRandomCode(8);
            pedido.QrCode.CodigoConfirmacao = code;

            pedido.Id = Repositorio.SaveAndReturn(pedido);

            return pedido;
        }
        
        public void Retirar(int id, string code, Usuario usuario)
        {
            var pedido = BuscarPorId(id);
            if (pedido == null)
                throw new Exception("PedidoVoucher Não Encontrado!");

            if (pedido.Status != StatusPedido.AguardandoRetirada)
                throw new Exception($"O pedido não pode ser finalizado. Status atual: {pedido.Status.ToDescription()}");

            if (!string.Equals(pedido.QrCode.CodigoConfirmacao, code, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("O QRCode capturado não confere com o pedido.");

            pedido.AddListaHistorico(StatusPedido.PedidoRetirado, usuario, pedido);

            pedido.AddListaHistorico(StatusPedido.PedidoFinalizado, usuario, pedido);

            Salvar(pedido);
        }

        public void AtribuiStatus(int id, int status, Usuario usuario, string descricao = null, string codigoRetornoTransacao = null)
        {
            var pedido = BuscarPorId(id);
            if (pedido == null)
                throw new Exception("PedidoVoucher não encontrado!");

            if (!Enum.IsDefined(typeof(StatusPedido), status))
                throw new Exception($"Status {status} não reconhecido.");

            pedido.AddListaHistorico((StatusPedido)status, usuario, pedido, descricao);
            Salvar(pedido);
        }

        public void ModificaStatus(int id, StatusPedido status, Usuario usuario, string descricao = null, string codigoRetornoTransacao = null)
        {
            var pedido = BuscarPorId(id);
            if (pedido == null)
                throw new Exception("PedidoVoucher não encontrado!");
            
            pedido.UpdListaHistorico(status, usuario, pedido, descricao);
            Salvar(pedido);
        }
        
        private void Valida(PedidoVoucher pedido)
        {
            if (pedido == null)
                throw new Exception("PedidoVoucher vazio ou nulo!<br/>Contate o suporte.");

            var lojaServico = ServiceLocator.Current.GetInstance<ILojaServico>();
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            pedido.Usuario = usuarioServico.BuscarPorId(pedido.Usuario.Id);
            
            pedido.Loja = pedido.Loja != null && pedido.Loja.Id > 0
                                ? lojaServico.BuscarPorId(pedido.Loja.Id) : null;
        }
    }
}