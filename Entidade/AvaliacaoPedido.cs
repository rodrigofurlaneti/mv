using Entidade.Base;
using System.Collections.Generic;

namespace Entidade
{
    public class AvaliacaoPedido : BaseEntity
    {
        public virtual bool ItensDeAcordoComAnuncio { get; set; }
        public virtual string Comentario { get; set; }
        public virtual int NotaAplicativo { get; set; }
        public virtual IList<AvaliacaoItemPedido> AvaliacaoItensPedido { get; set; }
        public virtual int NotaPedido { get; set; }
    }

    public class AvaliacaoItemPedido
    {
        public virtual ItemCompra ItemCompra { get; set; }
        public virtual bool ItemDeAcordoComAnuncio { get; set; }
        public virtual int Nota { get; set; }
        public virtual string Comentario { get; set; }
    }
}