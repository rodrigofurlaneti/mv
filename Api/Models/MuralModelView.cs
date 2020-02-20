using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Models
{
    public class MuralModelView
    {
        public MuralModelView()
        {

        }

        public MuralModelView(Mural mural)
        {
            Titulo = mural.Titulo;
            DataPublicacao = mural.DataPublicacao;
            FotoCapa = mural.FotoCapa;
            Descricao = mural.Descricao;
            Facebook = mural.Facebook;
            Comentarios = new List<ComentarioModelView>();
            foreach (var comentario in mural.Comentarios)
            {
                Comentarios.Add(new ComentarioModelView(comentario.Comentario));
            }
        }

        public string Titulo { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string FotoCapa { get; set; }
        public string Descricao { get; set; }
        public string ResumoDescricao { get { return Descricao.Substring(0, 100) + "..."; } }
        public string Facebook { get; set; }
        public string Data { get { return DataPublicacao.ToShortDateString(); } }

        public string TempoPublicacao
        {
            get
            {
                var hora = DateTime.Now.Subtract(DataPublicacao).TotalHours;

                return hora > 1 ? hora.ToString("#") + "hr" : DateTime.Now.Subtract(DataPublicacao).TotalMinutes.ToString("##") + "min";
            }
        }
        public IList<ComentarioModelView> Comentarios { get; set; }
        public int NumeroComentarios { get { return Comentarios.Count; } }
    }
}