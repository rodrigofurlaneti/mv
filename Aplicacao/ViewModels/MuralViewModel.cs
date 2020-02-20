using System;
using System.Collections.Generic;
using System.Linq;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class MuralViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string DataPublicacao { get; set; }
        public string FotoCapa { get; set; }
        public string Descricao { get; set; }
        public List<ComentarioMural> Comentarios { get; set; }
        public string FaceBook { get; set; }

        public static List<ComentarioMuralViewModel> ComentarioMuralViewModelList(IList<ComentarioMural> comentarios)
        {
            var comentariosVm = new List<ComentarioMuralViewModel>();
            if (comentarios == null || comentarios.Count <= 0) return comentariosVm;

            comentariosVm.AddRange(comentarios.Select(comentario => new ComentarioMuralViewModel(comentario)));
            return comentariosVm;
        }

        public MuralViewModel()
        {

        }

        public MuralViewModel(Mural entidade)
        {
            Id = entidade?.Id ?? 0;
            Titulo = entidade?.Titulo;
            DataPublicacao = entidade.DataPublicacao.ToString("dd/MM/yyyy");
            FotoCapa = entidade?.FotoCapa;
            Descricao = entidade?.Descricao;
            //this.Comentarios = ComentarioMuralViewModel.ComentariosMuralViewModelList(mural?.Comentarios.Select(x => x.Comentario).ToList() ?? new List<Comentarios>());
            FaceBook = entidade?.Facebook;
        }

        public Mural ToEntity() => new Mural()
        {
            Id = this.Id,
            Titulo = this.Titulo,
            DataPublicacao = string.IsNullOrEmpty(this.DataPublicacao) ? DateTime.Now : DateTime.Parse(this.DataPublicacao),
            FotoCapa = this.FotoCapa,
            Descricao = this.Descricao,
            Comentarios = this.Comentarios,
            Facebook = this.FaceBook
        };
    }
}