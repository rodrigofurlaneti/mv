using Core.Serializers;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;

namespace ApiInfox.Models
{
    public class HistoricoPedidoModelView
    {
        public HistoricoPedidoModelView(HistoricoPedido historicoPedido)
        {
            StatusPedido = historicoPedido.StatusPedido;
            Descricao = historicoPedido.Descricao;
            //UsuarioLogado = new UsuarioModelView(historicoPedido.UsuarioLogado);
            CodigoRetornoTransacao = historicoPedido.CodigoRetornoTransacao;
            //Pedido = new PedidoModelView(historicoPedido.Pedido);
        }

        [JsonConverter(typeof(EnumDescriptionStringConverter))]
        public virtual StatusPedido StatusPedido { get; set; }
        public virtual string Descricao { get; set; }
        //public virtual UsuarioModelView UsuarioLogado { get; set; }
        public virtual string CodigoRetornoTransacao { get; set; }
        //public virtual PedidoModelView Pedido { get; set; }
    }
}