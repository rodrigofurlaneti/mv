using Entidade.Base;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class Mural : BaseEntity
    {
        public Mural()
        {
            DataPublicacao = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public virtual string Titulo { get; set; }
        public virtual DateTime DataPublicacao { get; set; }
        public virtual string FotoCapa { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string Facebook { get; set; }

        public virtual IList<ComentarioMural> Comentarios { get; set; }
    }
}
