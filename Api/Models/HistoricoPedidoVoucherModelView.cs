using Core.Serializers;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;

namespace Api.Models
{
    public class HistoricoPedidoVoucherModelView
    {
        public HistoricoPedidoVoucherModelView(HistoricoPedidoVoucher historicoPedido)
        {
            StatusPedido = historicoPedido.StatusPedido;
            Descricao = historicoPedido.Descricao;
        }

        [JsonConverter(typeof(EnumDescriptionStringConverter))]
        public virtual StatusPedido StatusPedido { get; set; }
        public virtual string Descricao { get; set; }
    }
}