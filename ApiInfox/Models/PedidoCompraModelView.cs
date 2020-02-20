namespace ApiInfox.Models
{
    public class PedidoCompraModelView : ResponseModelView
    {
        public ClienteModelView Cliente { get; set; }
        public string ValorPedido { get; set; }
        public string CodEstabelecimento { get; set; }
    }
}