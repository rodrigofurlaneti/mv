using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entidade
{
    public class Veiculo : BaseEntity
    {
        public virtual string ModeloId { get; set; }

        public virtual string Modelo { get; set; }
        
        public virtual string Ano { get; set; }

        public virtual TipoVeiculo TipoVeiculo { get; set; }

        public virtual string TipoOutros { get; set; }

        public virtual string Placa { get; set; }

        public virtual Marca Marca { get; set; }

        public virtual bool MotoristaDeAplicativo { get; set; }

        public virtual bool Taxista { get; set; }

        public virtual Usuario Proprietario { get; set; }

        public virtual IList<Usuario> OutrosProprietarios { get; set; }

        public virtual string VeiculoFull
        {
            get
            {
                return String.Format("{0} {1}, {2}", "Placa: " + Placa, "Modelo: " + Modelo, "Marca: " + Marca.Nome);
            }
        }

        public virtual string ImagemUpload { get; set; }

        public virtual string Foto { get { return RetornarFoto(); } }

        public virtual string RetornarFoto()
        {
            return ImagemUpload != null && ImagemUpload.Any()
                        ? ImagemUpload
                        : "assets/image/tipoveiculo/" + TipoVeiculo.ToString() + ".png";
        }

    }
}