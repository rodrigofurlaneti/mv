using Entidade;
using System;

namespace Api.Models
{
    public class ComentarioModelView
    {
        public ComentarioModelView()
        {

        }

        public ComentarioModelView(Comentario comentario)
        {
            Usuario = comentario.Usuario?.Pessoa?.Nome;
            Titulo = comentario.Titulo;
            Descricao = comentario.Descricao;
            Estrelas = comentario.Estrelas;
            DataInsercao = comentario.DataInsercao;
        }

        public string Titulo { get; set; }
        public string Usuario { get; set; }
        public string FotoCapa { get; set; }
        public string Descricao { get; set; }
        public int Estrelas { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Data { get { return DataInsercao.ToShortDateString(); } }
        public string TempoPublicacao
        {
            get
            {
                var hora = DateTime.Now.Subtract(DataInsercao).TotalHours;

                return hora > 1 ? hora.ToString("#") + "hr" : DateTime.Now.Subtract(DataInsercao).TotalMinutes.ToString("##") + "min";
            }
        }
    }
}