using System.ComponentModel.DataAnnotations;
using Core.Serializers;
using Entidade.Base;
using Entidade.Uteis;
using Newtonsoft.Json;

namespace Entidade
{
    public class HistoricoPedido : BaseEntity
    {
        [JsonConverter(typeof(EnumDescriptionStringConverter))]
        [Required]
        public virtual StatusPedido StatusPedido { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Usuario UsuarioLogado { get; set; }
        public virtual string CodigoRetornoTransacao { get; set; }
        public virtual Pedido Pedido { get; set; }
    }
}