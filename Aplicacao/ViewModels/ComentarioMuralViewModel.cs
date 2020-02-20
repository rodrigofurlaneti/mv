using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class ComentarioMuralViewModel
    {
        public Mural Mural { get; set; }
        public Comentario Comentario { get; set; }

        public ComentarioMuralViewModel()
        {
        }

        public ComentarioMuralViewModel(ComentarioMural _comentario)
        {
            Mural = _comentario.Mural;
            Comentario = _comentario.Comentario;
        }

        public ComentarioMural ToEntity() => new ComentarioMural
        {
            Mural = this.Mural,
            Comentario = this.Comentario
            
        };        
    }
}