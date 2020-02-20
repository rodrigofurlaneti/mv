namespace Entidade
{
    public class ComentarioMural
    {
        public virtual Mural Mural { get; set; }
        public virtual Comentario Comentario { get; set; }
    }
}
