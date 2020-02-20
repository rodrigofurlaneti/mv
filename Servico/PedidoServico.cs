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
using Pedido = Entidade.Pedido;

namespace Dominio
{
    public interface IPedidoServico : IBaseServico<Pedido>
    {
        Pedido ValidarESalvar(Pedido entity);
        void Retirar(int id, string code, Usuario usuario);
        void AtribuiStatus(int id, int status, Usuario usuario, string descricao = null, string codigoRetornoTransacao = null);
        void ModificaStatus(int id, StatusPedido status, Usuario usuario, string descricao = null, string codigoRetornoTransacao = null);
        string SolicitarRetirada(int id);
        IEnumerable<Pedido> BuscarPorUsuario(Usuario usuario);
        void SalvarPedidoComAvaliacao(Pedido pedido);
    }

    public class PedidoServico : BaseServico<Pedido, IPedidoRepositorio>, IPedidoServico
    {
        public Pedido ValidarESalvar(Pedido pedido)
        {
            Valida(pedido);
            
            pedido.AddListaHistorico(StatusPedido.AguardandoPagamento, pedido.Usuario, pedido);
            pedido.Id = Repositorio.SaveAndReturn(pedido);
            pedido = Repositorio.GetById(pedido.Id);

            var qrcodeServico = ServiceLocator.Current.GetInstance<IQrCodeServico>();
            pedido.QrCode = qrcodeServico.GeraUrlESalva(pedido);
            pedido.Id = Repositorio.SaveAndReturn(pedido);

            return pedido;
        }
        
        public void Retirar(int id, string code, Usuario usuario)
        {
            var pedido = BuscarPorId(id);
            if (pedido == null)
                throw new Exception("Pedido Não Encontrado!");

            if (pedido.Status != StatusPedido.AguardandoRetirada && pedido.Status != StatusPedido.PedidoEnviado)
                throw new Exception($"O pedido não pode ser finalizado. Status atual: {pedido.Status.ToDescription()}");

            if (!string.Equals(pedido.QrCode.CodigoConfirmacao, code, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("O QRCode capturado não confere com o pedido.");

            pedido.AddListaHistorico(StatusPedido.PedidoRetirado, usuario, pedido);

            pedido.AddListaHistorico(StatusPedido.AguardandoAvaliacao, usuario, pedido);

            Salvar(pedido);
        }

        public void AtribuiStatus(int id, int status, Usuario usuario, string descricao = null, string codigoRetornoTransacao = null)
        {
            var pedido = BuscarPorId(id);
            if (pedido == null)
                throw new Exception("Pedido não encontrado!");

            if (!Enum.IsDefined(typeof(StatusPedido), status))
                throw new Exception($"Status {status} não reconhecido.");

            pedido.AddListaHistorico((StatusPedido)status, usuario, pedido, descricao, codigoRetornoTransacao);
            Salvar(pedido);
        }

        public void ModificaStatus(int id, StatusPedido status, Usuario usuario, string descricao = null, string codigoRetornoTransacao = null)
        {
            var pedido = BuscarPorId(id);
            if (pedido == null)
                throw new Exception("Pedido não encontrado!");
            
            pedido.UpdListaHistorico(status, usuario, pedido, descricao, codigoRetornoTransacao);
            Salvar(pedido);
        }

        public string SolicitarRetirada(int id)
        {
            var pedido = BuscarPorId(id);
            if (pedido == null)
                throw new Exception("Pedido não encontrado!");

            if (pedido.Status != StatusPedido.AguardandoRetirada)
                throw new Exception($"O pedido não pode ser retirado. Status atual: {pedido.Status.ToDescription()}");

            var code = Tools.GenerateRandomCode(8);
            pedido.QrCode.CodigoConfirmacao = code;

            var qrCodeServico = ServiceLocator.Current.GetInstance<IQrCodeServico>();
            qrCodeServico.Salvar(pedido.QrCode);

            // TODO: Ao invés de retornar, deve enviar um sms com o código.
            return code;
        }

        public IEnumerable<Pedido> BuscarPorUsuario(Usuario usuario)
        {
            var cartaoServico = ServiceLocator.Current.GetInstance<ICartaoServico>();
            var pedidos = BuscarPor(x => x.Usuario.Id.Equals(usuario.Id));
            pedidos?.ToList()?.ForEach(p =>
            {
                var decrypted = cartaoServico.DescriptografarCartao(p.Cartao);
                p.Cartao.Cvv = decrypted.Cvv;
                p.Cartao.Numero = decrypted.Numero;
            });
            return pedidos?.OrderByDescending(x => x.DataInsercao);
        }

        private void Valida(Pedido pedido)
        {
            if (pedido == null)
                throw new Exception("Pedido vazio ou nulo!<br/>Contate o suporte.");

            var cartaoServico = ServiceLocator.Current.GetInstance<ICartaoServico>();
            var agendamentoServico = ServiceLocator.Current.GetInstance<IAgendamentoServico>();
            var enderecoServico = ServiceLocator.Current.GetInstance<IEnderecoServico>();
            var lojaServico = ServiceLocator.Current.GetInstance<ILojaServico>();
            var listaCompraServico = ServiceLocator.Current.GetInstance<IListaCompraServico>();
            var usuarioServico = ServiceLocator.Current.GetInstance<IUsuarioServico>();
            pedido.Usuario = usuarioServico.BuscarPorId(pedido.Usuario.Id);

            if (pedido.Cartao == null)
                throw new Exception("Informe um cartão para finalizar o pedido.");

            if (pedido.Endereco == null && pedido.Agendamento == null)
                throw new Exception("Informe um endereço de entrega ou um agendamento para finalizar o pedido.");

            pedido.Loja = pedido.Loja != null && pedido.Loja.Id > 0
                                ? lojaServico.BuscarPorId(pedido.Loja.Id) : null;

            pedido.Cartao = cartaoServico.Criptografar(cartaoServico.BuscarPorId(pedido.Cartao.Id));

            pedido.Agendamento = pedido.Agendamento != null && pedido.Agendamento.Id > 0
                                    ? agendamentoServico.BuscarPorId(pedido.Agendamento.Id) : null;

            pedido.Endereco = pedido.Endereco != null && pedido.Endereco.Id > 0
                                ? enderecoServico.BuscarPorId(pedido.Endereco.Id) : null;

            pedido.ListaCompra = listaCompraServico.BuscarPorId(pedido?.ListaCompra?.Id ?? 0);

            if (pedido.ListaCompra == null)
                return;

            if ((pedido?.ListaCompra?.Id ?? 0) == 0)
                throw new Exception("Não foi encontrado a lista de compras, contate o suporte!");

            if (pedido.ListaCompra == null)
                throw new Exception("Informe uma lista de compras para finalizar o pedido.");
        }

        public void SalvarPedidoComAvaliacao(Pedido pedido)
        {
            pedido.AddListaHistorico(StatusPedido.PedidoFinalizado, pedido.Usuario, pedido);
            Repositorio.Save(pedido);
        }
    }
}