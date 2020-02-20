using System;
using EntidadePcSist.Base;

namespace EntidadePcSist
{
    public class PcEst : IEntityPcSist
    {
        public virtual int CodFilial { get; set; }
        public virtual int  CodProd { get; set; }
        public virtual int  QtEst { get; set; }
        public virtual decimal CustoReal { get; set; }
        public virtual DateTime DtUltEnt { get; set; }
        public virtual DateTime DtUltSaida { get; set; }

    }
}
