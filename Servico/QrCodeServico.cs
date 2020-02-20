using System;
using System.Configuration;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IQrCodeServico : IBaseServico<QrCode>
    {
        QrCode GeraUrlESalva(Pedido pedido);
        QrCode GeraUrlESalva(PedidoVoucher pedido);
    }

    public class QrCodeServico : BaseServico<QrCode, IQrCodeRepositorio>, IQrCodeServico
    {
        public QrCode GeraUrlESalva(Pedido pedido)
        {
            var host = ConfigurationManager.AppSettings["HOST"];
            if(string.IsNullOrWhiteSpace(host))
                throw new Exception("Não foi possível encontrar a chave HOST.");

            var qrCode = new QrCode { Url = pedido.Id.ToString() };
            Repositorio.Save(qrCode);
            return qrCode;
        }

        public QrCode GeraUrlESalva(PedidoVoucher pedido)
        {
            var host = ConfigurationManager.AppSettings["HOST"];
            if (string.IsNullOrWhiteSpace(host))
                throw new Exception("Não foi possível encontrar a chave HOST.");

            var qrCode = new QrCode { Url = pedido.Id.ToString() };
            Repositorio.Save(qrCode);
            return qrCode;
        }
    }
}